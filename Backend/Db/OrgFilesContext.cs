using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Backend.Db
{  
    public class OrgFilesContext : DbContext
    {
        public DbSet<OrgFile> OrgFiles { get; set; }

        protected static string GetConnectionStringByName(string name)
        {
            var builder = new ConfigurationBuilder();
            var config = builder.AddJsonFile("db.json").Build();
            return config.GetSection("ConnectionStrings").GetSection(name).Get<string>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string conn = GetConnectionStringByName("orgfiledb");
            builder.UseNpgsql(conn);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}