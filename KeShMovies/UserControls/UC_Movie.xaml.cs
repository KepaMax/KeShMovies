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


    public string Poster
    {
        get { return (string)GetValue(PosterProperty); }
        set { SetValue(PosterProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Poster.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PosterProperty =
        DependencyProperty.Register("Poster", typeof(string), typeof(UC_Movie));



    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(UC_Movie));




    public string imdbRating
    {
        get { return (string)GetValue(imdbRatingProperty); }
        set { SetValue(imdbRatingProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ImdbRating.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty imdbRatingProperty =
        DependencyProperty.Register("imdbRating", typeof(string), typeof(UC_Movie));



    public string Year
    {
        get { return (string)GetValue(YearProperty); }
        set { SetValue(YearProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Year.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty YearProperty =
        DependencyProperty.Register("Year", typeof(string), typeof(UC_Movie));

    public UC_Movie()
    {
        InitializeComponent();
    }


}
