using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TurtBOT
{
    public static class DbHelper
    {
        public async static Task<DbUser> GetUser(BotDbContext db, ulong id)
        {
            var user = await db.Users.FindAsync(id);
            if (user is null)
            {
                var newUser = new DbUser
                {
                    Id = id
                };
                await db.Users.AddAsync(newUser);
                await db.SaveChangesAsync();
                return newUser;
            }
            return user;
        }
    }
}
