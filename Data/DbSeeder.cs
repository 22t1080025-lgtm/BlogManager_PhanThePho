using BlogManager_PhanThePho.Models;

namespace BlogManager_PhanThePho.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
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
    }
}