using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Database;

public class EfDbContext : DbContext
{
    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<BaseChat> Chats { get; set; } = null!;
    public DbSet<PrivateChat> PrivateChats { get; set; } = null!;
    public DbSet <GroupChat> GroupChats { get; set; } = null!;
    public DbSet<PrivateChatUser> PrivateChatUsers { get; set; } = null!;
    public DbSet<GroupChatUser> GroupChatUsers { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<FileContent> FileContents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseChat>()
            .HasDiscriminator<string>("Type")
            .HasValue<GroupChat>("Group")
            .HasValue<PrivateChat>("Private");

        modelBuilder.Entity<PrivateChat>()
            .HasMany(e => e.Members)
            .WithMany(e => e.PrivateChats)
            .UsingEntity<PrivateChatUser>();

        modelBuilder.Entity<GroupChatUser>()
            .HasKey(e => new { e.GroupChatId, e.MemberId });

        modelBuilder.Entity<FileContent>()
            .HasKey(e => e.MessageId);
    }
}
