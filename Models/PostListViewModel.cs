namespace BlogManager_PhanThePho.Models;

public class PostListViewModel
{
    public List<Post> Posts { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
}