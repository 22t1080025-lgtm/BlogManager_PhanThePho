using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Thêm namespace này
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogManager_PhanThePho.Models;

namespace BlogManager_PhanThePho.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }

        // Thêm DbSet này để quản lý bảng Categories
        public DbSet<Category> Categories { get; set; }
    }
}
