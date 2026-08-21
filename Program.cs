using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Thêm namespace này
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

// 3. Thêm Razor Pages (Cần thiết cho giao diện Identity UI mặc định)
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Truyền trực tiếp 'services' vào thay vì 'context'
    await BlogManager_PhanThePho.Data.DbSeeder.SeedAsync(services); 
}

app.UseHttpsRedirection();
app.UseRouting();

// 4. Bật Authentication (Xác thực) và Authorization (Phân quyền)
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// 5. Route cho các trang Đăng nhập / Đăng xuất của Identity UI
app.MapRazorPages();

app.Run();