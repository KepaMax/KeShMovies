using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeShMovies.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? WatchList { get; set; }
    public string? Favorites { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is User other)
        {
            return Email == other.Email && Username == other.Username && Password == other.Password;
        }

        return false;
    }

}