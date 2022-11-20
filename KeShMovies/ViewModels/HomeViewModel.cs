using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public ObservableCollection<Movie> Movies { get; set; }

    public string? SearchText { get; set; }
    public User CurrentUser { get; set; }
    public HomeViewModel(User currentUser, NavigationStore navigationStore)
    {
        CurrentUser = currentUser;
        _navigationStore = navigationStore;
        Movies = new();
        var jsonstr =  OmdbService.GetMovieByTitle("Avengers Endgame");
        var movie = JsonSerializer.Deserialize<Movie>(jsonstr);
        Movies.Add(movie);
    }
}
