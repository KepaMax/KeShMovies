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
    private readonly User _currentUser;

    public ObservableCollection<Movie> History { get; set; }
    public ICommand LoadCommand { get; set; }
    public ICommand UndoCommand { get; set; }
    public ICommand OpenFullInfoCommand { get; set; }



    public HistoryViewModel(User currentUser, BaseViewModel previousViewModel, NavigationStore navigationStore, IUserRepository userRepository)
    {
        _previousViewModel = previousViewModel;
        _navigationStore = navigationStore;
        _currentUser = currentUser;
        _userRepository = userRepository;
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
        History.Clear();

        if (string.IsNullOrWhiteSpace(_currentUser.History)) return;

        var history = _currentUser.History.TrimEnd(';').Split(';');

        for (int i = history.Length - 1; i >= 0; i--)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(history[i]);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);

            if (movie?.Poster == "N/A")
                movie.Poster = "/StaticFiles/Images/no-image-icon-6.png";

            if (movie is not null)
            {
                History.Add(movie);
            }
        }
    }

    private async void ExecuteOpenFullInfoCommand(object? parametr)
    {
        if (parametr is UC_MovieSimplified Movie)
        {
            var movieJson = await OmdbService.GetConcreteMovieById(Movie.ImdbId);
            var movie = JsonSerializer.Deserialize<Movie>(movieJson);
            if (movie is not null)
                _navigationStore.CurrentViewModel = new MovieInfoViewModel(movie, _currentUser, this, _navigationStore);
        }

    }
}
