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
    // ==================== UPDATE (SỬA) ====================

    // GET: Posts/Edit/5 (Hiển thị form sửa bài viết)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        return View(post);
    }

    // POST: Posts/Edit/5 (Lưu dữ liệu chỉnh sửa)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Post post)
    {
        if (id != post.Id) return NotFound();

        if (!ModelState.IsValid) return View(post);

        _context.Update(post);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ==================== DELETE (XÓA) ====================

    // GET: Posts/Delete/5 (Hiển thị trang xác nhận xóa)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        return View(post);
    }

    // POST: Posts/Delete/5 (Thực hiện xóa bài viết)
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
    // Các Action ở đây
}