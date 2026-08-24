using System.ComponentModel.DataAnnotations;

namespace BlogManager_PhanThePho.Models;

public class Post
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Tiêu đề phải từ 3 đến 200 ký tự")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nội dung không được để trống")]
    public string Content { get; set; } = string.Empty;

    public DateTime PublishedAt { get; set; } = DateTime.Now;

    public bool IsPublished { get; set; }

    public string Author { get; set; } = string.Empty;
    public int ViewCount { get; set; }

    public string MoTaNgan() => $"{Title} ({PublishedAt:dd/MM/yyyy})";

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}