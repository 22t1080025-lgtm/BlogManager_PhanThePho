using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogManager_PhanThePho.Models;
using BlogManager_PhanThePho.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogManager_PhanThePho.Controllers;

[Authorize] // Bắt buộc đăng nhập cho toàn bộ Controller
public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Cho phép tất cả mọi người xem danh sách
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, string? sort, int pageNumber = 1)
    {
        int pageSize = 5;
        
        var query = _context.Posts.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Title.Contains(search));
        }

        query = sort switch
        {
            "title" => query.OrderBy(p => p.Title),
            "oldest" => query.OrderBy(p => p.PublishedAt),
            _ => query.OrderByDescending(p => p.PublishedAt)
        };

        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var posts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new PostListViewModel
        {
            Posts = posts,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            Search = search,
            Sort = sort
        };

        return View(viewModel);
    }

    // Cho phép tất cả mọi người xem chi tiết
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null) return NotFound();

        return View(post);
    }

    // GET: Posts/Create (Cần đăng nhập - thừa hưởng từ [Authorize] ở class)
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    // POST: Posts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Posts/Edit/5 (Cần đăng nhập - thừa hưởng từ [Authorize] ở class)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        ViewBag.Categories = await _context.Categories.ToListAsync();

        return View(post);
    }

    // POST: Posts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Post post)
    {
        if (id != post.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }

        _context.Update(post);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Posts/Delete/5 (CHỈ DÀNH CHO ADMIN)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null) return NotFound();

        return View(post);
    }

    // POST: Posts/Delete/5 (CHỈ DÀNH CHO ADMIN)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}