using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace KeShMovies.ViewModels;

public class SignUpViewModel : BaseViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly NavigationStore _navigationStore;
    private readonly List<User>? _users;

    public static string PasswordHelperText { get; set; } = @"At Least 8 Characters
At Least 1 UpperCase
At Least 1 Number";
    public static string UsernameHelperText { get; set; } = @"At Least 3 Characters
At Least 1 UpperCase";

    public ICommand SignUpCommand { get; set; }
    public ICommand GoLogInCommand { get; set; }

    private static readonly string usernameRegex = @"^(?=[a-zA-Z])[-\w.]{2,23}([a-zA-Z\d]|(?<![-.])_)$";
    private static readonly string passwordRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$";

    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;

    private Notifier _notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
            corner: Corner.TopRight,
            offsetX: 10,
            offsetY: 10);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(3));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    public SignUpViewModel(IUserRepository userRepository, NavigationStore navigationStore)
    {

        _userRepository = userRepository;
        _navigationStore = navigationStore;

        _users = _userRepository.GetList() != null ? _userRepository.GetList() : new();

        SignUpCommand = new RelayCommand(ExecuteSignUpCommand);
        GoLogInCommand = new RelayCommand(ExecuteGoLogInCommandCommand);
    }

    private void ExecuteGoLogInCommandCommand(object? obj)
    {
        _navigationStore.CurrentViewModel = new LogInViewModel(_userRepository, _navigationStore);
    }

    private void ExecuteSignUpCommand(object? parametr)
    {
        var user = new User()
        {
            Email = this.Email.Trim(),
            Username = this.Username.Trim(),
            Password = this.Password.Trim()
        };




        if(!IsValidEmail())
        {
            _notifier.ShowError($"Incorrect {nameof(Email)} Format");
            return;
        }    
        
        if(!Regex.IsMatch(Username, usernameRegex))
        {
            _notifier.ShowError($"Incorrect {nameof(Username)} Format");
            return;
        }
        
        if (!Regex.IsMatch(Password, passwordRegex))
        {
            _notifier.ShowError($"Incorrect {nameof(Password)} Format");
            return;
        }

        if (_users is not null)
        {
            if (_users.Any(u => u.Email == user.Email))
            {
                _notifier.ShowError($"This {nameof(Email)} Is Already Used");
                return;
            }

            if (_users.Any(u => u.Username == user.Username))
            {
                _notifier.ShowError($"This {nameof(Username)} Is Already Used");
                return;
            }

            _notifier.ShowSuccess($"Succesfully Signed Up!");

            _users.Add(user);
            _userRepository.Add(user);

            _navigationStore.CurrentViewModel = new HomeViewModel(user, _navigationStore, _userRepository);
            return;
        }

    }

    private bool IsValidEmail()
    {
        if (string.IsNullOrWhiteSpace(Email))
            return false;

        try
        {
            MailAddress m = new MailAddress(Email);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
