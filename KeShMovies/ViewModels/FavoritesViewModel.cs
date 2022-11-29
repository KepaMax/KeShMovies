using KeShMovies.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeShMovies.ViewModels;

public class FavoritesViewModel
{
    public User CurrentUser { get; set; }

    public ObservableCollection<Movie> Favorites { get; set; }

    public FavoritesViewModel(User currentUser)
    {
        CurrentUser = currentUser;
        Favorites = new();
    }

}
