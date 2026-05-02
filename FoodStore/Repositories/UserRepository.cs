using FoodStore.Data;
using FoodStore.Interfaces;
using FoodStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(user => user.Username == usernameOrEmail || user.Email == usernameOrEmail);
        return user;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        bool exists = await _context.Users.AnyAsync(user => user.Username == username);
        return exists;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        bool exists = await _context.Users.AnyAsync(user => user.Email == email);
        return exists;
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
