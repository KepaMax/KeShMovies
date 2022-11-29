using KeShMovies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeShMovies.UserControls;

public partial class UC_Movie : UserControl
{
    public event EventHandler<EventArgs>? AddToFavorites;
    public event EventHandler<EventArgs>? RemoveFromFavorites;

    public string ImdbId
    {
        get { return (string)GetValue(ImdbIdProperty); }
        set { SetValue(ImdbIdProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ImdbId.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ImdbIdProperty =
        DependencyProperty.Register("ImdbId", typeof(string), typeof(UC_Movie));



    public string Poster
    {
        get { return (string)GetValue(PosterProperty); }
        set { SetValue(PosterProperty, value); }
    }

    public static readonly DependencyProperty PosterProperty =
        DependencyProperty.Register("Poster", typeof(string), typeof(UC_Movie));



    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(UC_Movie));




    public bool IsFavorite
    {
        get { return (bool)GetValue(IsFavoriteProperty); }
        set { SetValue(IsFavoriteProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsFavorite.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsFavoriteProperty =
        DependencyProperty.Register("IsFavorite", typeof(bool), typeof(UC_Movie));



    public string imdbRating
    {
        get { return (string)GetValue(imdbRatingProperty); }
        set { SetValue(imdbRatingProperty, value); }
    }

    public static readonly DependencyProperty imdbRatingProperty =
        DependencyProperty.Register("imdbRating", typeof(string), typeof(UC_Movie));



    public string Year
    {
        get { return (string)GetValue(YearProperty); }
        set { SetValue(YearProperty, value); }
    }

    public static readonly DependencyProperty YearProperty =
        DependencyProperty.Register("Year", typeof(string), typeof(UC_Movie));

    public UC_Movie()
    {
        InitializeComponent();
    }

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        AddToFavorites?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
        RemoveFromFavorites?.Invoke(this, EventArgs.Empty);
    }
}
