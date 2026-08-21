using BlogManager_PhanThePho.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogManager_PhanThePho.Controllers;

public class LabController : Controller
{
    public IActionResult Index()
    {
        var baiViet = new List<Post>
        {
            new Post { Id = 1, Title = "C# cơ bản", IsPublished = true },
            new Post { Id = 2, Title = "MVC nhập môn", IsPublished = false },
            new Post { Id = 3, Title = "EF Core", IsPublished = true },

            new Post { Id = 4, Title = "Lập trình Web với ASP.NET Core", Author = "Trần Thị B", ViewCount = 500, IsPublished = true },
            new Post { Id = 5, Title = "Hướng dẫn Git & GitHub", Author = "Lê Văn C", ViewCount = 220, IsPublished = true },
            new Post { Id = 6, Title = "Kỹ thuật Clean Code", Author = "Phan Thế Phố", ViewCount = 120, IsPublished = false }
        };

        // Yêu cầu 1: Bài đã xuất bản (IsPublished == true), sắp xếp theo ViewCount giảm dần
        ViewBag.BaiDaXuatBan = baiViet
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.ViewCount)
            .ToList();

        // Yêu cầu 2: Tổng số lượt xem tất cả bài viết
        ViewBag.TongLuotXem = baiViet.Sum(p => p.ViewCount);

        // Yêu cầu 3: Bài viết có nhiều lượt xem nhất
        ViewBag.BaiVietXemNhieuNhat = baiViet
            .OrderByDescending(p => p.ViewCount)
            .FirstOrDefault();

        /*ViewBag.SoDaXuatBan = baiViet.Count(p => p.IsPublished);

        ViewBag.TieuDe = baiViet
            .Where(p => p.IsPublished)
            .OrderBy(p => p.Title)
            .Select(p => p.Title)
            .ToList();
        */
        return View();
    }
}