using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using KeShMovies.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
    private readonly User _currentUser;
    public Movie Movie { get; set; }
    public ICommand UndoCommand { get; set; }
    public ICommand PlayTrailerCommand { get; set; }

    public MovieInfoViewModel(Movie movie, User currentUser, BaseViewModel previousViewModel, NavigationStore navigationStore)
    {
        _previousViewModel = previousViewModel;
        _currentUser = currentUser;
        _navigationStore = navigationStore;

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
        if (!_currentUser.History.Contains(Movie.imdbID))
        {
            _currentUser.History += Movie.imdbID + ';';
        }
        else
        {
            var changedId = Movie.imdbID + ';';
            var startIndex = _currentUser.History.IndexOf(changedId);
            _currentUser.History = _currentUser.History.Remove(startIndex, changedId.Length) + changedId;
        }

        NameValueCollection nameValueCollection = new NameValueCollection();
        nameValueCollection.Add("q", Movie.Title);
        var webClient = new WebClient();
        webClient.QueryString.Add(nameValueCollection);

        var youtubesearch = new ProcessStartInfo
        {
            FileName = "https://www.youtube.com/results?search_query=" + Movie.Title + " trailer",
            UseShellExecute = true
        };
        Process.Start(youtubesearch);
    }

}
