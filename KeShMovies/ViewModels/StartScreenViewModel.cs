using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace KeShMovies.ViewModels
{
    public class StartScreenViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
        private readonly NavigationStore _navigationStore;

        public StartScreenViewModel(IUserRepository userRepository, NavigationStore navigationStore)
        {
            _userRepository = userRepository;
            _navigationStore = navigationStore;

            System.Timers.Timer timer = new();
            timer.Interval = 7500;
            timer.Elapsed += Timer_Elapsed; ;
            timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (sender is System.Timers.Timer timer)
            {
                timer.Stop();
                timer.Dispose();
                _navigationStore.CurrentViewModel = new LogInViewModel(_userRepository, _navigationStore);
            }
        }

    }
}
