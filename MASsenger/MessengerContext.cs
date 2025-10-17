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
		public DbSet<DirectChat> DirectChats { get; set; } = null!;
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

			// modelBuilder.Entity<Channel>()
			// 	.HasOne(e => e.LinkedGroup)
			// 	.WithOne(e => e.LinkedChannel)
			// 	.HasForeignKey<LinkedGroup>(e => e.GroupId);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Members)
                .WithMany(e => e.Groups);

            modelBuilder.Entity<Group>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.GroupsOwned)
                .OnDelete(DeleteBehavior.Restrict);

			// modelBuilder.Entity<Group>()
			// 	.HasOne(e => e.LinkedChannel)
			// 	.WithOne(e => e.LinkedGroup)
			// 	.HasForeignKey<LinkedChannel>(e => e.ChannelId);

            modelBuilder.Entity<Bot>()
                .HasMany(e => e.Members)
                .WithMany(e => e.Bots);

            modelBuilder.Entity<Bot>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.BotsOwned)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			//    optionsBuilder.UseSqlServer("Server=localhost;Database=MASsengerDB;Trusted_Connection=True;TrustServerCertificate=True;");
			optionsBuilder.UseSqlite("Filename=MASsengerDB.db");
        }
    }
}
