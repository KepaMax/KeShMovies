using KeShMovies.Models;
using System;
using System.Collections.Generic;
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


    public Movie Movie
    {
        get { return (Movie)GetValue(MovieProperty); }
        set { SetValue(MovieProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Movie.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MovieProperty =
        DependencyProperty.Register(nameof(Movie), typeof(Movie), typeof(UC_Movie));



    public UC_Movie(Movie movie)
    {
        InitializeComponent();
        Movie = movie;
    }


}
