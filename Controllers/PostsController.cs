using Microsoft.AspNetCore.Mvc;
using BlogManager_PhanThePho.Models;
using BlogManager_PhanThePho.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogManager_PhanThePho.Controllers;

public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Posts (Index có Include, Tìm kiếm, Sắp xếp và Phân trang)
    public async Task<IActionResult> Index(string? search, string? sort, int pageNumber = 1)
    {
        int pageSize = 5;
        
        // 1. Nạp kèm bảng Category bằng Include
        var query = _context.Posts.Include(p => p.Category).AsQueryable();

        // 2. Lọc theo từ khóa tìm kiếm
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Title.Contains(search));
        }

        // 3. Sắp xếp danh sách
        query = sort switch
        {
            "title" => query.OrderBy(p => p.Title),
            "oldest" => query.OrderBy(p => p.PublishedAt),
            _ => query.OrderByDescending(p => p.PublishedAt)
        };

        // 4. Tính toán phân trang với Skip & Take
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var posts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 5. Đóng gói vào ViewModel
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

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null) return NotFound();

        return View(post);
    }

    // GET: Posts/Create
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

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        // Thêm dòng này để nạp danh sách chuyên mục vào ViewBag
        ViewBag.Categories = await _context.Categories.ToListAsync();

        return View(post);
    }

    // POST: Posts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
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

    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null) return NotFound();

        return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
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