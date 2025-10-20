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
        public DbSet<BaseUser> BaseUsers { get; set; } = null!;
        public DbSet<BaseChat> BaseChats { get; set; } = null!;
        public DbSet<BaseMessage> BaseMessages { get; set; } = null!;

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bot>()
                .HasMany(e => e.Members)
                .WithMany(e => e.BotsJoined);

            modelBuilder.Entity<Bot>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.BotsOwned)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelGroupChat>()
                .HasMany(e => e.Members)
                .WithMany(e => e.ChannelGroupsJoined);

            modelBuilder.Entity<ChannelGroupChat>()
                .HasMany(e => e.Admins)
                .WithMany(e => e.ChannelGroupsManaged);

            modelBuilder.Entity<ChannelGroupChat>()
                .HasMany(e => e.Banned)
                .WithMany(e => e.ChannelGroupsBannedFrom);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //    optionsBuilder.UseSqlServer("Server=localhost;Database=MASsengerDB;Trusted_Connection=True;TrustServerCertificate=True;");
            optionsBuilder.UseSqlite("Filename=MASsengerDB.db");
        }
    }
}
