using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using KeShMovies.Services;
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
    public User CurrentUser { get; set; }
    public ObservableCollection<Movie> History { get; set; }
    public ICommand LoadCommand { get; set; }
    public ICommand UndoCommand { get; set; }
    


    public HistoryViewModel(User currentUser, NavigationStore navigationStore, IUserRepository userRepository)
    {
        _navigationStore = navigationStore;
        CurrentUser = currentUser;
        _userRepository= userRepository;
        History = new();

        LoadCommand = new RelayCommand(ExecuteLoadCommand);
        UndoCommand = new RelayCommand(ExecuteUndoCommand);
    }

    private void ExecuteUndoCommand(object? parametr)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(CurrentUser, _navigationStore, _userRepository);
    }

    private async void ExecuteLoadCommand(object? parametr)
    {
        if (string.IsNullOrWhiteSpace(CurrentUser.Favorites)) return;

        var favorites = CurrentUser.History.TrimEnd(';').Split(';');

        for (int i = favorites.Length - 1; i >= 0; i--)
        {
            var movieJson = await OmdbService.GetConcreteMovie(favorites[i]);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);

            if (movie.Poster == "N/A")
                movie.Poster = "/StaticFiles/Images/Movie Logo.gif";

            if (movie is not null)
                History.Add(movie);
        }

    }
}
