using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public ICommand LoadCommand { get; set; }

    public User CurrentUser { get; set; }

    public ObservableCollection<Movie> Favorites { get; set; }

    public FavoritesViewModel(User currentUser, NavigationStore navigationStore)
    {
        CurrentUser = currentUser;
        Favorites = new();
        _navigationStore = navigationStore;


        LoadCommand = new RelayCommand(ExecuteLoadCommand);
    }

    private async void ExecuteLoadCommand(object? obj)
    {
        var favorites = CurrentUser.Favorites.TrimEnd(';').Split(';');
        MessageBox.Show(favorites.Length.ToString());

        foreach (var movieId in favorites)
        {
            var movieJson = await OmdbService.GetConcreteMovie(movieId);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);
                movie.IsFavorite = true;

            if (movie is not null)
                Favorites.Add(movie);
        }
    }
}
