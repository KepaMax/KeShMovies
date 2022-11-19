using KeShMovies.Commands;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class SignUpViewModel : BaseViewModel
{
    //private readonly IUserRepository _userRepository;

    public ICommand ContinueCommand { get; set; }
    public ICommand LogInCommand { get; set; }

    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;

    public SignUpViewModel(/*IUserRepository userRepository*/)
    {
        //_userRepository = userRepository;

        ContinueCommand = new RelayCommand(ExecuteContinueCommand, CanExecuteContinueCommand);
        LogInCommand = new RelayCommand(ExecuteLogInCommand);
    }

    private void ExecuteLogInCommand(object? obj)
    {
        throw new NotImplementedException();
    }

    private void ExecuteContinueCommand(object? parametr)
    {
        MessageBox.Show($"{Email} {Username} {Password}");
    }

    private bool CanExecuteContinueCommand(object? parametr)
    {
        return true;
    }
}
