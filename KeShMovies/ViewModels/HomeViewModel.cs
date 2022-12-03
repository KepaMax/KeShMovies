using Autofac;
using DevExpress.Mvvm.Native;
using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using KeShMovies.Services;
using KeShMovies.UserControls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace KeShMovies.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly IUserRepository _userRepository;
    private readonly User _currentUser;
    private readonly Notifier _notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
            corner: Corner.TopRight,
            offsetX: 10,
            offsetY: 10);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(3));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    public ObservableCollection<Movie> Movies { get; set; }
    public ICommand SearchCommand { get; set; }
    public ICommand LogOutCommand { get; set; }
    public ICommand AddToFavoritesCommand { get; set; }
    public ICommand RemoveFromFavoritesCommand { get; set; }
    public ICommand SwitchToFavoritesCommand { get; set; }
    public ICommand SwitchToHistoryCommand { get; set; }
    public ICommand OpenFullInfoCommand { get; set; }

    public string? SearchText { get; set; }

    public HomeViewModel(User currentUser, NavigationStore navigationStore, IUserRepository userRepository, string defaultSearch = null)
    {
        _currentUser = currentUser;
        _navigationStore = navigationStore;
        _userRepository = userRepository;
        Movies = new();

        SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
        LogOutCommand = new RelayCommand(ExecuteLogOutCommand);
        AddToFavoritesCommand = new RelayCommand(ExecuteAddToFavoritesCommand);
        RemoveFromFavoritesCommand = new RelayCommand(ExecuteRemoveFromFavoritesCommand);
        SwitchToFavoritesCommand = new RelayCommand(ExecuteSwitchToFavoritesCommand);
        SwitchToHistoryCommand = new RelayCommand(ExecuteSwitchToHistoryCommand);
        OpenFullInfoCommand = new RelayCommand(ExecuteOpenFullInfoCommand);

        if (defaultSearch != null)
        {
            SearchText = defaultSearch;
            ExecuteSearchCommand(new object());
        }

    }

    private async void ExecuteOpenFullInfoCommand(object? parametr)
    {
        if (parametr is UC_Movie Movie)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(Movie.ImdbId);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);

            
            _userRepository.Update(_currentUser);
            _navigationStore.CurrentViewModel = new MovieInfoViewModel(movie, _currentUser, this, _navigationStore);
        }

    }

    private void ExecuteSwitchToHistoryCommand(object? obj) => _navigationStore.CurrentViewModel = new HistoryViewModel(_currentUser, this, _navigationStore, _userRepository);
    private void ExecuteLogOutCommand(object? parametr) => _navigationStore.CurrentViewModel = App.Container?.Resolve<LogInViewModel>();
    private void ExecuteSwitchToFavoritesCommand(object? obj) => _navigationStore.CurrentViewModel = new FavoritesViewModel(_currentUser, _navigationStore, _userRepository, SearchText);
    private bool CanExecuteSearchCommand(object? parametr) => !string.IsNullOrWhiteSpace(SearchText);


    private void ExecuteRemoveFromFavoritesCommand(object? parametr)
    {
        if (parametr is UC_Movie movie && _currentUser is not null)
        {
            var removeId = movie.ImdbId + ';';
            var startIndex = _currentUser.Favorites.IndexOf(removeId);

            if (_currentUser.Favorites.Contains(movie.ImdbId))
            {
                _currentUser.Favorites = _currentUser.Favorites.Remove(startIndex, removeId.Length);
                _userRepository.Update(_currentUser);
            }
        }
    }


    private void ExecuteAddToFavoritesCommand(object? parametr)
    {
        if (parametr is UC_Movie movie && _currentUser is not null)
        {
            if (!_currentUser.Favorites.Contains(movie.ImdbId))
            {
                _currentUser.Favorites += movie.ImdbId + ';';
                _notifier.ShowSuccess($"{movie.Title} Added\nMove To Favorites To See All");
                _userRepository.Update(_currentUser);
            }
        }
    }


    private async void ExecuteSearchCommand(object? parametr)
    {

        var jsonStr = await OmdbService.GetAllMoviesByTitle(SearchText);

        var movies = JsonSerializer.Deserialize<MovieCollection>(jsonStr);

        Movies.Clear();

        if (movies.Search is not null)
        {
            foreach (var result in movies.Search)
            {
                var movieCollectionJson = await OmdbService.GetConcreteMovieById(result.imdbID);

                var movieFromCollection = JsonSerializer.Deserialize<Movie>(movieCollectionJson);
                if (_currentUser.Favorites.Contains(movieFromCollection.imdbID))
                    movieFromCollection.IsFavorite = true;

                if (movieFromCollection.Poster == "N/A")
                    movieFromCollection.Poster = "\\StaticFiles\\Images\\no-image-icon-6.png";

                if (movieFromCollection is not null)
                    Movies.Add(movieFromCollection);
            }

            return;
        }

        var movieJson = await OmdbService.GetConcreteMovieByTitle(SearchText);
        var movie = JsonSerializer.Deserialize<Movie>(movieJson);
        if (movie?.imdbID is not null)
            Movies.Add(movie);

    }
}
