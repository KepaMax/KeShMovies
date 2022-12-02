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
    public ICommand LoadCommand { get; set; }
    public ICommand RemoveFromFavoritesCommand { get; set; }
    public ICommand AddToFavoritesCommand { get; set; }
    public ICommand UndoCommand { get; set; }

    public User CurrentUser { get; set; }

    public ObservableCollection<Movie> Favorites { get; set; }

    public FavoritesViewModel(User currentUser, NavigationStore navigationStore, IUserRepository userRepository)
    {
        CurrentUser = currentUser;
        Favorites = new();
        _navigationStore = navigationStore;


        LoadCommand = new RelayCommand(ExecuteLoadCommand);
        RemoveFromFavoritesCommand = new RelayCommand(ExecuteRemoveCommand);
        AddToFavoritesCommand = new RelayCommand(ExecuteAddToFavoritesCommand);
        UndoCommand = new RelayCommand(ExecuteUndoCommand);
        _userRepository = userRepository;
    }

    private void ExecuteUndoCommand(object? parametr)
    {
        _navigationStore.CurrentViewModel = new HomeViewModel(CurrentUser, _navigationStore, _userRepository);
    }
    private void ExecuteAddToFavoritesCommand(object? parametr)
    {
        if (parametr is UC_Movie movie && CurrentUser is not null)
        {
            if (!CurrentUser.Favorites.Contains(movie.ImdbId))
            {
                CurrentUser.Favorites += movie.ImdbId + ';';
                _userRepository.Update(CurrentUser);
            }

        }
    }
    private async void ExecuteLoadCommand(object? parametr)
    {
        if (string.IsNullOrWhiteSpace(CurrentUser.Favorites)) return;

        var favorites = CurrentUser.Favorites.TrimEnd(';').Split(';');
        

        foreach (var movieId in favorites)
        {
            var movieJson = await OmdbService.GetConcreteMovie(movieId);

            var movie = JsonSerializer.Deserialize<Movie>(movieJson);
            movie.IsFavorite = true;

            if (movie.Poster == "N/A")
                movie.Poster = "/StaticFiles/Images/Movie Logo.gif";

            if (movie is not null)
                Favorites.Add(movie);
        }
    }

    private void ExecuteRemoveCommand(object? parametr)
    {

        if (parametr is UC_Movie movie && CurrentUser is not null)
        {
            var removeId = movie.ImdbId + ';';
            var startIndex = CurrentUser.Favorites.IndexOf(removeId);

            if (CurrentUser.Favorites.Contains(movie.ImdbId))
            {
                CurrentUser.Favorites = CurrentUser.Favorites.Remove(startIndex, removeId.Length);
                _userRepository.Update(CurrentUser);
            }


        }



    }



}



