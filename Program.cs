using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogManager_PhanThePho.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// 3. Thêm Razor Pages (Cho Identity UI)
builder.Services.AddRazorPages();

// 4. Đăng ký Swagger / OpenAPI cho API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Bật Swagger & Swagger UI ở môi trường Development
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5. Khởi tạo Seed Data (Tạo danh mục + Admin)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await BlogManager_PhanThePho.Data.DbSeeder.SeedAsync(services); 
}

app.UseHttpsRedirection();
app.UseRouting();

// 6. Bật Authentication & Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// 7. Route cho Identity UI
app.MapRazorPages();

// 8. Ánh xạ các API Controllers (BẮT BUỘC để không bị lỗi API 404)
app.MapControllers();

app.Run();