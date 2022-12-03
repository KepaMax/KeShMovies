using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using KeShMovies.Services;
using KeShMovies.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace KeShMovies.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly NavigationStore _navigationStore;
    private readonly User _currentUser;

    private readonly string? _searchText;
    public ICommand LoadCommand { get; set; }
    public ICommand RemoveFromFavoritesCommand { get; set; }
    public ICommand AddToFavoritesCommand { get; set; }
    public ICommand OpenFullInfoCommand { get; set; }
    public ICommand UndoCommand { get; set; }


    public ObservableCollection<Movie> Favorites { get; set; }

    public FavoritesViewModel(User currentUser, NavigationStore navigationStore, IUserRepository userRepository, string? searchText)
    {
        _searchText = searchText;
        _currentUser = currentUser;
        Favorites = new();
        _navigationStore = navigationStore;


        LoadCommand = new RelayCommand(ExecuteLoadCommand);
        RemoveFromFavoritesCommand = new RelayCommand(ExecuteRemoveCommand);
        AddToFavoritesCommand = new RelayCommand(ExecuteAddToFavoritesCommand);
        UndoCommand = new RelayCommand(ExecuteUndoCommand);
        OpenFullInfoCommand = new RelayCommand(ExecuteOpenFullInfoCommand);
        _userRepository = userRepository;
    }
    private void ExecuteUndoCommand(object? parametr) => _navigationStore.CurrentViewModel = new HomeViewModel(_currentUser, _navigationStore, _userRepository, _searchText);

    private async void ExecuteOpenFullInfoCommand(object? parametr)
    {
        if (parametr is UC_Movie Movie)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(Movie.ImdbId);
            var movie = JsonSerializer.Deserialize<Movie>(movieJson);
            _navigationStore.CurrentViewModel = new MovieInfoViewModel(movie, _currentUser, this, _navigationStore);
        }

    }


    private void ExecuteAddToFavoritesCommand(object? parametr)
    {
        if (parametr is UC_Movie movie && _currentUser is not null)
        {
            if (!_currentUser.Favorites.Contains(movie.ImdbId))
            {
                _currentUser.Favorites += movie.ImdbId + ';';
                _userRepository.Update(_currentUser);
            }

        }
    }

    private async void ExecuteLoadCommand(object? parametr)
    {
        Favorites.Clear();

        if (string.IsNullOrWhiteSpace(_currentUser.Favorites)) return;

        var favorites = _currentUser.Favorites.TrimEnd(';').Split(';');


        for (int i = favorites.Length - 1; i >= 0; i--)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(favorites[i]);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);
            if (movie is not null)
            {
                movie.IsFavorite = true;

                if (movie.Poster == "N/A")
                    movie.Poster = "/StaticFiles/Images/no-image-icon-6.png";

                Favorites.Add(movie);
            }

        }

    }

    private void ExecuteRemoveCommand(object? parametr)
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



}



