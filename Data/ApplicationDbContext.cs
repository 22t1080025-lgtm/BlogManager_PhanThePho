using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogManager_PhanThePho.Models;

namespace BlogManager_PhanThePho.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }

        // Thêm DbSet này để quản lý bảng Categories
        public DbSet<Category> Categories { get; set; }
    }
}
