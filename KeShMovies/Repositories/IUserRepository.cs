using KeShMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeShMovies.Repositories;

public interface IUserRepository : IRepository<User>
{
    User? GetById(Guid Id);
}
