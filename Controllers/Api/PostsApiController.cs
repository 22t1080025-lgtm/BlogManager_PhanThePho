using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BlogManager_PhanThePho.Data;
using BlogManager_PhanThePho.Models;
using BlogManager_PhanThePho.Dtos;

namespace BlogManager_PhanThePho.Controllers.Api;

[ApiController]
[Route("api/posts")]
public class PostsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PostsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        var posts = await _context.Posts
            .Include(p => p.Category)
            .Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                PublishedAt = p.PublishedAt,
                CategoryName = p.Category != null ? p.Category.Name : null
            })
            .ToListAsync();

        return Ok(posts); // 200
    }

    // GET: api/posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        var p = await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return NotFound(); // 404

        var dto = new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            PublishedAt = p.PublishedAt,
            CategoryName = p.Category != null ? p.Category.Name : null
        };

        return Ok(dto); // 200
    }

    // POST: api/posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(PostCreateDto dto)
    {
        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            CategoryId = dto.CategoryId,
            PublishedAt = DateTime.Now
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var result = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            PublishedAt = post.PublishedAt
        };

        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, result); // 201
    }

    // PUT: api/posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, PostCreateDto dto)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound(); // 404

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent(); // 204
    }

    // DELETE: api/posts/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound(); // 404

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent(); // 204
    }
}