using KeShMovies.Views;
using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KeShMovies.ViewModels;
using KeShMovies.Navigation;
using KeShMovies.Repositories;
using KeShMovies.Models;
using System.Threading;

namespace KeShMovies;

public partial class App : Application
{
    public static IContainer? Container { get; private set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        NavigationStore navigationStore = new();
        var builder = new ContainerBuilder();

        builder.RegisterInstance(navigationStore).SingleInstance();

        builder.RegisterType<MainViewModel>();
        builder.RegisterType<HomeViewModel>();
        builder.RegisterType<SignUpViewModel>();
        builder.RegisterType<LogInViewModel>();
        builder.RegisterType<StartScreenViewModel>();
        builder.RegisterType<EfUserRepository>().As<IUserRepository>();
        

        Container = builder.Build();

        navigationStore.CurrentViewModel = Container.Resolve<StartScreenViewModel>();

        MainView mainView = new();
        mainView.DataContext= Container.Resolve<MainViewModel>();



        mainView.Show();
    }
}
