using Microsoft.EntityFrameworkCore;
using StudentWebService.Models;

namespace StudentWebService.Data
{
    public class StudentAPIDbContext : DbContext
    {
        public StudentAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Grupa> Grupa { get; set; }

        public DbSet<Materie> Materie { get; set; }

        public DbSet<Oras> Orase { get; set; }

        public DbSet<Note> Note { get; set; }

    }
}
