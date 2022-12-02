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
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class HistoryViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly IUserRepository _userRepository;
    private readonly BaseViewModel _previousViewModel;
    public User CurrentUser { get; set; }
    public string Tooltip { get; set; } = null;
    public ObservableCollection<Movie> History { get; set; }
    public ICommand LoadCommand { get; set; }
    public ICommand UndoCommand { get; set; }
    public ICommand OpenFullInfoCommand { get; set; }
    


    public HistoryViewModel(User currentUser, BaseViewModel previousViewModel, NavigationStore navigationStore, IUserRepository userRepository)
    {
        _previousViewModel = previousViewModel;

        _navigationStore = navigationStore;
        CurrentUser = currentUser;
        _userRepository= userRepository;
        History = new();

        LoadCommand = new RelayCommand(ExecuteLoadCommand);
        UndoCommand = new RelayCommand(ExecuteUndoCommand);
        OpenFullInfoCommand = new RelayCommand(ExecuteOpenFullInfoCommand);
    }

    private void ExecuteUndoCommand(object? parametr)
    {
        _navigationStore.CurrentViewModel = _previousViewModel;
    }

    private async void ExecuteLoadCommand(object? parametr)
    {
        if (string.IsNullOrWhiteSpace(CurrentUser.History)) return;

        var favorites = CurrentUser.History.TrimEnd(';').Split(';');

        for (int i = favorites.Length - 1; i >= 0; i--)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(favorites[i]);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);

            if (movie?.Poster == "N/A")
                movie.Poster = "/StaticFiles/Images/no-image-icon-6.png";

            if (movie is not null)
            {
                History.Add(movie);
                Tooltip = DateTime.Now.ToString();
            }
        }
    }

    private async void ExecuteOpenFullInfoCommand(object? parametr)
    {
        if (parametr is UC_MovieSimplified Movie)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(Movie.ImdbId);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);

            if (!CurrentUser.History.Contains(movie.imdbID))
            {
                CurrentUser.History += movie.imdbID + ';';
            }
            else
            {
                var changedId = movie.imdbID + ';';
                var startIndex = CurrentUser.History.IndexOf(changedId);
                CurrentUser.History = CurrentUser.History.Remove(startIndex, changedId.Length) + changedId;
            }
            _userRepository.Update(CurrentUser);
            _navigationStore.CurrentViewModel = new MovieInfoViewModel(movie, this, _navigationStore);
        }

    }
}
