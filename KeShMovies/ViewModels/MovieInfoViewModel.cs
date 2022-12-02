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
using System.Windows;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class MovieInfoViewModel : BaseViewModel
{

    private readonly NavigationStore _navigationStore;
    private readonly BaseViewModel _previousViewModel;
    public Movie Movie { get; set; }
    public ICommand UndoCommand { get; set; }
    public ICommand PlayTrailerCommand { get; set; }

    public MovieInfoViewModel(Movie movie,BaseViewModel previousViewModel, NavigationStore navigationStore)
    {
        _previousViewModel = previousViewModel;

        _navigationStore=navigationStore;
        Movie = movie;
        if (Movie.Poster == "N/A")
            Movie.Poster = "/StaticFiles/Images/Movie Logo.gif";

        PlayTrailerCommand = new RelayCommand(ExecutePlayTrailerCommand);
        UndoCommand = new RelayCommand(ExecuteUndoCommand);
    }

    private void ExecuteUndoCommand(object? parametr)
    {
        _navigationStore.CurrentViewModel = _previousViewModel;
    }


    private void ExecutePlayTrailerCommand(object? parametr)
    {
        
    }

}
