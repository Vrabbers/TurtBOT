#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace TurtBOT
{
    public static class DbHandler 
    {
        public static async Task<DbUser> GetUser(ulong id)
        {
            await using var d = new BotDbContext();
            var dbUsers = await d.Users.ToListAsync();
            if (!dbUsers.Exists(u => u.DbUserId == id) || dbUsers.Count == 0)
            { 
                var user = new DbUser(id);
                await d.AddAsync(user);
                await d.SaveChangesAsync();
                return user;
            }

            return await d.Users.FirstAsync(u => u.DbUserId == id);

        }

        public static async Task ChangeUserNickname(ulong id, string nickname)
        {
            await using var d = new BotDbContext();
            var user = await GetUser(id);
            user.DefinedNickname = nickname;
            d.Users.Attach(user);
            d.Entry(user).Property(x => x.DefinedNickname).IsModified = true;
            await d.SaveChangesAsync();
        }
        
    }
    public class BotDbContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>().HasKey(a => a.DbUserId);
            base.OnModelCreating(modelBuilder);
        }

        public void CreateIfNotExists()
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                databaseCreator.CreateTables();
            }
            catch (SqliteException)
            {
                Console.WriteLine("tables already exist");
            } //if there are tables, itll throw. so you just ignore
        }
    }

    public class DbUser
    {
        [Required]
        public ulong DbUserId { get; set; }
        public string? DefinedNickname { get; set; }

        public DbUser(ulong dbUserId)
        {
            DbUserId = dbUserId;
        }
    }
    
}