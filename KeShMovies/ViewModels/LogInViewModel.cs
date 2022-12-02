using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class LogInViewModel : BaseViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly NavigationStore _navigationStore;
    private readonly List<User>? _users;

    public ICommand LogInCommand { get; set; }
    public ICommand GoSigUpCommand { get; set; }
    public string UsernameOrEmail { get; set; } = default!;
    public string Password { get; set; } = default!;

    public LogInViewModel(IUserRepository userRepository, NavigationStore navigationStore)
    {
        _userRepository = userRepository;
        _navigationStore = navigationStore;
        _users = _userRepository.GetList() != null ? _userRepository.GetList() : new();

        LogInCommand = new RelayCommand(ExecuteLogInCommand, CanExecuteLogInCommand);
        GoSigUpCommand = new RelayCommand(ExecuteGoSigUpCommand);
    }

    private void ExecuteGoSigUpCommand(object? obj)
    {
        _navigationStore.CurrentViewModel = new SignUpViewModel(_userRepository, _navigationStore);
    }

    private void ExecuteLogInCommand(object? parametr)
    {
        User? user = null;

        if (IsValidEmail())
        {
            user = _users?.Find(u => u.Email == UsernameOrEmail);

            if (user is not null)
            {
                if (user.Password == Password)
                {
                    _navigationStore.CurrentViewModel = new HomeViewModel(user, _navigationStore, _userRepository);
                    return;
                }
                MessageBox.Show("Password Don't Match");
                return;
            }
            MessageBox.Show("Email Not Found");
            return;
        }



        user = _users?.Find(u => u.Username == UsernameOrEmail);
        if (user is not null)
        {
            if (user.Password == Password)
            {
                _navigationStore.CurrentViewModel = new HomeViewModel(user, _navigationStore,_userRepository);
                return;
            }
            MessageBox.Show("Password Don't Match");
            return;
        }

        MessageBox.Show("Username Not Found");
    }


    private bool CanExecuteLogInCommand(object? parametr)
    {
        if (parametr is StackPanel sp)
        {
            foreach (var txt in sp.Children.OfType<TextBox>())
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                    return false;
            }

            return true;
        }

        return false;
    }

    private bool IsValidEmail()
    {
        if (UsernameOrEmail is null)
            return false;

        try
        {
            MailAddress m = new MailAddress(UsernameOrEmail);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}
