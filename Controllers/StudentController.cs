using Microsoft.AspNetCore.Mvc;
using AspKutuphane.Data; // Veritabanı context'in burada yer alıyor
using AspKutuphane.Models;
using Microsoft.EntityFrameworkCore;

namespace AspKutuphane.Controllers
{
    public class StudentController : Controller
    {
        private readonly LibraryContext _context;

        // Veritabanı bağlantısını Constructor ile alıyoruz
        public StudentController(LibraryContext context)
        {
            _context = context;
        }

        // Öğrencileri Listeleme Sayfası (Index)
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.BorrowedBooks)
                .ToListAsync();
            return View(students);
        }

        // Öğrenci Detay Sayfası (Details)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .Include(s => s.BorrowedBooks)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null) return NotFound();

            return View(student);
        }
    }
}  