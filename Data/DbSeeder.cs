using BlogManager_PhanThePho.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BlogManager_PhanThePho.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider service)
    {
        var context = service.GetRequiredService<ApplicationDbContext>();

        // 1. Thêm danh mục mặc định
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Âm nhạc" },
                new Category { Name = "Điện ảnh" },
                new Category { Name = "Game" },
                new Category { Name = "Ăn uống" },
                new Category { Name = "Thể thao" }
            };
            
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // 2. Tạo tài khoản Admin mặc định
        var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
        
        var defaultUser = new IdentityUser
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true
        };

        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
            await userManager.CreateAsync(defaultUser, "Admin@123");
        }
    }
}