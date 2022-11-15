using KeShMovies.Models;
using KeShMovies.UserControls;
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
using System.Windows.Shapes;

namespace KeShMovies.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            grid.Children.Add(new UC_Movie(new Movie() { Poster = "https://yandex.ru/images/search?pos=0&from=tabbar&img_url=http%3A%2F%2Fi.pinimg.com%2Foriginals%2F18%2F03%2F7f%2F18037fac0dffb95aaea8a02048137d39.jpg&text=slash+poster&rpt=simage&lr=10253" }));
        }
    }
}
