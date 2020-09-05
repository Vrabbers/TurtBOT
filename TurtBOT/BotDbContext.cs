using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TurtBOT
{
    public class BotDbContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=" + DataDirectory.Combine("database.db"));
        
    }

    public class DbUser
    { 
        public ulong Id { get; set; }
        public decimal FavouriteNumber { get; set; }
    }
}
