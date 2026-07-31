namespace BlogManager_PhanThePho.Models;

public class Posts
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Content { get; set; } = "";

    public string Author { get; set; } = "";

    public DateTime PublishedAt { get; set; }

    public bool IsPublished { get; set; }
}