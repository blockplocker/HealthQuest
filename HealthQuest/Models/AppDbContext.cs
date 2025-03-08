using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Models
{
    class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<DailyQuest> DailyQuests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=tcp:freesqlnoa.database.windows.net,1433;Initial Catalog=noadb;Persist Security Info=False;User ID=noadb;Password=Hemligt123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=HealthQuest;Trusted_Connection=True;TrustServerCertificate=True;");   //local  

        }
    }
}
