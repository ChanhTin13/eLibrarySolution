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
        public DbSet<tbl_Author> tbl_Author { get; set; }
        public DbSet<tbl_Title> tbl_Title { get; set; }
        public DbSet<tbl_Category> tbl_Category { get; set; }
        public DbSet<tbl_Book> tbl_Book { get; set; }
        public DbSet<tbl_LibraryLoan> tbl_LibraryLoan { get; set; }
        #region store model
        #endregion
    }
}
