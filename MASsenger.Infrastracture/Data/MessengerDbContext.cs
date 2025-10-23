using MASsenger.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Infrastracture.Data
{
    public class MessengerDbContext : DbContext
    {
        public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options)
        {
            
        }

        public DbSet<BaseUser> BaseUsers { get; set; } = null!;
        public DbSet<BaseChat> BaseChats { get; set; } = null!;
        public DbSet<BaseMessage> BaseMessages { get; set; } = null!;

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
    }
}
