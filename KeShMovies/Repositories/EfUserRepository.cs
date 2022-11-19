using KeShMovies.Models;
using KeShMovies.Repositories.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeShMovies.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly MyDbContext _context;

    public EfUserRepository() => _context = new();

    public void Add(User entity)
    {
        _context.Users?.Add(entity);
        _context.SaveChanges();
    }

    public User? Get(Func<User, bool> predicate) => _context.Users?.FirstOrDefault(predicate);

    public User? GetById(Guid Id) => _context.Users?.Find(Id);

    public List<User>? GetList(Func<User, bool>? predicate = null)
    => (predicate is null) switch
    {
        true => _context.Users?.ToList(),
        false => _context.Users?.Where(predicate).ToList()
    };

    public void Remove(User entity)
    {
        _context.Users?.Remove(entity);
        _context.SaveChanges();
    }

    public void Update(User entity)
    {
        _context.Users?.Update(entity);
        _context.SaveChanges();
    }
}
