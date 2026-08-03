using Microsoft.AspNetCore.Mvc;
using BlogManager_PhanThePho.Models;
using BlogManager_PhanThePho.Data;
using Microsoft.EntityFrameworkCore;

public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _context.Posts
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync();

        return View(posts);
    }
    public async Task<IActionResult> Details(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound();

        return View(post);
    }
    public IActionResult Create()
        {
            return View();
        }
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    // Các Action ở đây
}