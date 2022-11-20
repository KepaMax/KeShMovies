using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public string? SearchText { get; set; }
    public User CurrentUser { get; set; }

    public ICommand TestCommand { get; set; }

    public HomeViewModel(User currentUser, NavigationStore navigationStore)
    {
        CurrentUser = currentUser;
        _navigationStore = navigationStore;

        TestCommand = new RelayCommand(ExecuteTestCommand);
    }

    private void ExecuteTestCommand(object? parametr)
    {
        if(parametr is TextBox txt && !string.IsNullOrEmpty(txt.Text))
        {
            MessageBox.Show(txt.Text);
        }
    }
}
