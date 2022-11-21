using Autofac;
using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Services;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public ObservableCollection<Movie> Movies { get; set; }
    public ICommand SearchCommand { get; set; }
    public ICommand LogOutCommand { get; set; }
    
    public string? SearchText { get; set; }
    public User CurrentUser { get; set; }

    public HomeViewModel(User currentUser, NavigationStore navigationStore)
    {
        CurrentUser = currentUser;
        _navigationStore = navigationStore;
        Movies = new();

        SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
        LogOutCommand = new RelayCommand(ExecuteLogOutCommand);
    }

    private void ExecuteLogOutCommand(object? parametr)
    {
        _navigationStore.CurrentViewModel = App.Container?.Resolve<LogInViewModel>();
    }

    private bool CanExecuteSearchCommand(object? parametr) => !string.IsNullOrWhiteSpace(SearchText);

    private async void ExecuteSearchCommand(object? parametr)
    {

        var jsonStr = await OmdbService.GetAllMoviesByTitle(SearchText);

        var movies = JsonSerializer.Deserialize<MovieCollection>(jsonStr);

        if (movies.Search is not null)
        {
            Movies.Clear();
            foreach (var result in movies.Search)
            {
                var movieJson = await OmdbService.GetConcreteMovie(result.imdbID);

                var movie = JsonSerializer.Deserialize<Movie>(movieJson);
                if (movie is not null)
                    Movies.Add(movie);
            }
        }
            

    }
}
