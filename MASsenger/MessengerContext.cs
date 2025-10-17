using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger
{
    internal class MessengerContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Bot> Bots { get; set; } = null!;

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .HasMany(e => e.Members)
                .WithMany(e => e.Channels);

            modelBuilder.Entity<Channel>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.ChannelsOwned)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=MASsengerDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}