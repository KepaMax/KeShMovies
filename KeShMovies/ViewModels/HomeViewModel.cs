using Autofac;
using DevExpress.Mvvm.Native;
using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Services;
using KeShMovies.UserControls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public ObservableCollection<Movie> Movies { get; set; }
    public ICommand SearchCommand { get; set; }
    public ICommand LogOutCommand { get; set; }
    public ICommand AddToFavoritesCommand { get; set; }
    public ICommand RemoveFromFavoritesCommand { get; set; }

    public string? SearchText { get; set; }
    public User CurrentUser { get; set; }

    public HomeViewModel(User currentUser, NavigationStore navigationStore)
    {
        CurrentUser = currentUser;
        _navigationStore = navigationStore;
        Movies = new();

        SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
        LogOutCommand = new RelayCommand(ExecuteLogOutCommand);
        AddToFavoritesCommand = new RelayCommand(ExecuteAddToFavoritesCommand);
        RemoveFromFavoritesCommand = new RelayCommand(ExecuteRemoveFromFavoritesCommand);
    }

    private void ExecuteRemoveFromFavoritesCommand(object? parametr)
    {
        if (parametr is UC_Movie movie && CurrentUser is not null)
        {
            var removeId = movie.ImdbId + ';';
            var startIndex = CurrentUser.Favorites.IndexOf(removeId);

            if (CurrentUser.Favorites.Contains(movie.ImdbId))
                CurrentUser.Favorites=CurrentUser.Favorites.Remove(startIndex, removeId.Length);

        }
    }

    private void ExecuteLogOutCommand(object? parametr) => _navigationStore.CurrentViewModel = App.Container?.Resolve<LogInViewModel>();

    private void ExecuteAddToFavoritesCommand(object? parametr)
    {
        if (parametr is UC_Movie movie && CurrentUser is not null)
        {
            if (!CurrentUser.Favorites.Contains(movie.ImdbId))
                CurrentUser.Favorites += movie.ImdbId + ';';
        }
    }

    private bool CanExecuteSearchCommand(object? parametr) => !string.IsNullOrWhiteSpace(SearchText);

    private async void ExecuteSearchCommand(object? parametr)
    {

        var jsonStr = await OmdbService.GetAllMoviesByTitle(SearchText);

        var movies = JsonSerializer.Deserialize<MovieCollection>(jsonStr);

        Movies.Clear();

        if (movies.Search is not null)
        {
            foreach (var result in movies.Search)
            {
                var movieJson = await OmdbService.GetConcreteMovie(result.imdbID);

                var movie = JsonSerializer.Deserialize<Movie>(movieJson);
                if (CurrentUser.Favorites.Contains(movie.imdbID)) 
                    movie.IsFavorite = true;

                if (movie is not null)
                    Movies.Add(movie);
            }
        }


    }
}
