using Microsoft.AspNetCore.Mvc;
using BlogManager_PhanThePho.Models;

public class PostsController : Controller
{
    // Hàm trả về danh sách bài viết
    private List<Posts> getListPost()
    {
        var posts = new List<Posts>
        {
            new Posts
            {
                Id = 1,
                Title = "C# cơ bản",
                Content = "Giới thiệu ngôn ngữ C# và cú pháp cơ bản.",
                Author = "Nguyễn Văn A",
                PublishedAt = new DateTime(2024, 5, 5),
                IsPublished = true
            },
            new Posts
            {
                Id = 2,
                Title = "MVC nhập môn",
                Content = "Làm quen với mô hình MVC trong ASP.NET Core.",
                Author = "Trần Thị B",
                PublishedAt = new DateTime(2024, 6, 5),
                IsPublished = true
            },
            new Posts
            {
                Id = 3,
                Title = "EF Core",
                Content = "Tìm hiểu Entity Framework Core và cách thao tác CSDL.",
                Author = "Lê Văn C",
                PublishedAt = new DateTime(2024, 7, 5),
                IsPublished = false
            }
        };

        return posts;
    }

    public IActionResult Index()
    {
        var posts = getListPost();

        ViewData["Title"] = "Danh sách bài viết";
        ViewBag.SoLuong = posts.Count;

        return View(posts);
    }

    public IActionResult Details(int id)
    {
        var posts = getListPost();

        var baiCanLay = posts.FirstOrDefault(p => p.Id == id);

        if (baiCanLay != null)
            return View(baiCanLay);

        return NotFound();
    }
}