using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenr.Models;

namespace UrlShortenr.Data
{
    public class UrlContext : DbContext
    {
        public virtual DbSet<Url> Urls { get; set; }
        public virtual DbSet<Statistic> Statistics { get; set; }

        public UrlContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=Urldb;Uid=root;Pwd=1234");
        }
    }
}
