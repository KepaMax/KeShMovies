using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

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

    public LogInViewModel(IUserRepository userRepository, NavigationStore navigationStore)
    {
        _userRepository = userRepository;
        _navigationStore = navigationStore;
        _users = _userRepository.GetList() != null ? _userRepository.GetList() : new();

        LogInCommand = new RelayCommand(ExecuteLogInCommand);
        GoSigUpCommand = new RelayCommand(ExecuteGoSigUpCommand);
    }

    private void ExecuteGoSigUpCommand(object? obj)
    {
        _navigationStore.CurrentViewModel = new SignUpViewModel(_userRepository, _navigationStore);
    }

    private void ExecuteLogInCommand(object? parametr)
    {
        User? user = null;
        UsernameOrEmail = UsernameOrEmail.Trim();

        if (IsValidEmail())
        {
            user = _users?.Find(u => u.Email == UsernameOrEmail);

            if (user is not null)
            {
                if (user.Password == Password)
                {
                    _notifier.ShowSuccess($"{user.Username} Succesfully Logged In");
                    _navigationStore.CurrentViewModel = new HomeViewModel(user, _navigationStore, _userRepository);
                    return;
                }
                _notifier.ShowError("Password Don't Match");
                return;
            }
            _notifier.ShowError("Email Not Found");
            return;
        }



        user = _users?.Find(u => u.Username == UsernameOrEmail);
        if (user is not null)
        {
            if (user.Password == Password)
            {
                _notifier.ShowSuccess($"{user.Username} Succesfully Logged In");
                _navigationStore.CurrentViewModel = new HomeViewModel(user, _navigationStore,_userRepository);
                return;
            }
            _notifier.ShowError("Password Don't Match");
            return;
        }
        _notifier.ShowError("Username Not Found");
    }


    private bool IsValidEmail()
    {
        if (string.IsNullOrWhiteSpace(UsernameOrEmail))
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
