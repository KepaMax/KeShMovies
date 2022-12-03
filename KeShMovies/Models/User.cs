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
    public string History { get; set; } = string.Empty;
    public string Favorites { get; set; } = string.Empty;

}