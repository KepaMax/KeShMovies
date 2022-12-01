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

namespace KeShMovies.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UC_MovieSimplified.xaml
    /// </summary>
    public partial class UC_MovieSimplified : UserControl
    {
        public UC_MovieSimplified()
        {
            InitializeComponent();
        }

        public string ImdbId
        {
            get { return (string)GetValue(ImdbIdProperty); }
            set { SetValue(ImdbIdProperty, value); }
        }

        public static readonly DependencyProperty ImdbIdProperty =
            DependencyProperty.Register("ImdbId", typeof(string), typeof(UC_MovieSimplified));

        public string Poster
        {
            get { return (string)GetValue(PosterProperty); }
            set { SetValue(PosterProperty, value); }
        }

        public static readonly DependencyProperty PosterProperty =
            DependencyProperty.Register("Poster", typeof(string), typeof(UC_MovieSimplified));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(UC_MovieSimplified));

        public string imdbRating
        {
            get { return (string)GetValue(imdbRatingProperty); }
            set { SetValue(imdbRatingProperty, value); }
        }

        public static readonly DependencyProperty imdbRatingProperty =
            DependencyProperty.Register("imdbRating", typeof(string), typeof(UC_MovieSimplified));



        public string Year
        {
            get { return (string)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(string), typeof(UC_MovieSimplified));

    }
}
