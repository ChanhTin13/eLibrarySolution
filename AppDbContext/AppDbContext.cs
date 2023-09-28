using Microsoft.EntityFrameworkCore;
using Entities;
using Extensions;
using Interface.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDbContext
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<tbl_Wards> tbl_Wards { get; set; }
        public DbSet<tbl_Necessary> tbl_Necessary { get; set; }
        public DbSet<tbl_Districts> tbl_Districts { get; set; }
        public DbSet<tbl_Cities> tbl_Cities { get; set; }
        public DbSet<tbl_Users> tbl_Users { get; set; }
        public DbSet<tbl_Role> tbl_Role { get; set; }
        public DbSet<tbl_Authors> tbl_Authors { get; set; }
        public DbSet<tbl_Titles> tbl_Titles { get; set; }
        public DbSet<tbl_Books> tbl_Books { get; set; }
        #region store model
        #endregion
    }
}
