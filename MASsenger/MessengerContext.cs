using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    internal class MessengerContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Bot> Bots { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=MASsengerDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

