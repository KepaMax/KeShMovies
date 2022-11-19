using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeShMovies.ViewModels;

public class LogInViewModel :BaseViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly List<User>? _users;
    public string UsernameOrEmail { get; set; } = default!;
    public string Password { get; set; } = default!;

    public LogInViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _users = _userRepository.GetList() != null ? _userRepository.GetList() : new();
    }
}
