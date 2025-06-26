using Hackathon.AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.AuthService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}


