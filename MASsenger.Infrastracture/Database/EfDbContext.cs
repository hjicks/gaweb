using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Core.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Database
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
            
        }

        public DbSet<BaseUser> BaseUsers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Bot> Bots { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<BaseChat> BaseChats { get; set; } = null!;
        public DbSet<ChannelChat> ChannelChats { get; set; } = null!;
        public DbSet<PrivateChat> PrivateChats { get; set; } = null!;
        public DbSet<BaseMessage> BaseMessages { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<SystemMessage> SystemMessages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUser>()
                .HasDiscriminator<string>("Type")
                .HasValue<User>("User")
                .HasValue<Bot>("Bot");

            modelBuilder.Entity<BaseChat>()
                .HasDiscriminator<string>("Type")
                .HasValue<ChannelChat>("Channel")
                .HasValue<PrivateChat>("Private");

            modelBuilder.Entity<BaseMessage>()
                .HasDiscriminator<string>("Type")
                .HasValue<Message>("Message")
                .HasValue<SystemMessage>("SystemMessage");

            modelBuilder.Entity<Bot>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.BotsOwned)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bot>()
                .HasMany(e => e.Members)
                .WithMany(e => e.BotsJoined);

            modelBuilder.Entity<ChannelChat>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.ChannelsOwned)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelChat>()
                .HasMany(e => e.Members)
                .WithMany(e => e.ChannelsJoined)
                .UsingEntity(join => join.ToTable("ChannelsMembers"));

            modelBuilder.Entity<ChannelChat>()
                .HasMany(e => e.Admins)
                .WithMany(e => e.ChannelsManaged)
                .UsingEntity(join => join.ToTable("ChannelsAdmins"));

            modelBuilder.Entity<ChannelChat>()
                .HasMany(e => e.Banned)
                .WithMany(e => e.ChannelsBannedFrom)
                .UsingEntity(join => join.ToTable("ChannelsBannedUsers"));

            modelBuilder.Entity<PrivateChat>()
                .HasOne(e => e.Receiver)
                .WithMany(e => e.PrivateChats)
                .HasForeignKey(e => e.ReceiverId);
        }
    }
}
