using Microsoft.AspNetCore.Mvc;
using AspKutuphane.Data;
using AspKutuphane.Models;
using AspKutuphane.Repositories; // Repository katmanını kullanabilmek için ekledik
using Microsoft.EntityFrameworkCore;

namespace AspKutuphane.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryContext _context;
        private readonly BookRepository _repository; // Yeni eklediğimiz metotlar için entegre ettik

        // Constructor kısmında hem context'i hem de repository'yi içeri alıyoruz
        public AccountController(LibraryContext context, BookRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // --- ÜYE OLMA İŞLEMLERİ ---

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (exists)
                {
                    ModelState.AddModelError("", "Bu e-posta zaten kayıtlı.");
                    return View(user);
                }

                _context.Users.Add(user);

                var student = new Student
                {
                    StudentName = user.FullName,
                    StudentMail = user.Email,
                    Department = "Genel Üye",
                    BorrowedBooks = new List<Book>()
                };
                _context.Students.Add(student);

                await _context.SaveChangesAsync();

                CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Append("ActiveUserEmail", user.Email, option);

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // --- GİRİŞ YAPMA İŞLEMLERİ ---

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Append("ActiveUserEmail", user.Email, option);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "E-posta veya şifre hatalı!";
            return View();
        }

        // --- PROFIL VE ÇIKIŞ ---

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            string? email = Request.Cookies["ActiveUserEmail"];

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return RedirectToAction("Index", "Home");

            var student = await _context.Students
                .Include(s => s.BorrowedBooks)
                .FirstOrDefaultAsync(s => s.StudentMail == email);

            ViewBag.Student = student;
            return View(user);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("ActiveUserEmail");
            return RedirectToAction("Index", "Home");
        }

        // ==================================================================
        // HESAP AYARLARI PANELİ METOTLARI (ÇİFT MODEL YAPINA TAM UYUMLU)
        // ==================================================================

        // 1. Ayarlar Sayfasını Açan Metot (GET)
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var email = Request.Cookies["ActiveUserEmail"];
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");

            // Hem User hem de Student ilişkisini yönetebilmek için Student'ı çekiyoruz
            var student = await _repository.GetUserByEmailAsync(email);
            if (student == null) return NotFound();

            return View(student);
        }

        // 2. Bilgileri ve Şifreyi Güncelleyen Metot (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(Student model, string currentPassword, string newPassword)
        {
            var email = Request.Cookies["ActiveUserEmail"];
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");

            // Veri tabanından hem User kaydını hem de Student kaydını eş zamanlı buluyoruz
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var student = await _repository.GetUserByEmailAsync(email);

            if (user == null || student == null) return NotFound();

            // Güvenlik Kontrolü: Mevcut girilen şifre User tablosundakiyle uyuşuyor mu?
            if (user.Password != currentPassword)
            {
                ModelState.AddModelError("", "Mevcut şifreniz hatalı!");
                return View(student);
            }

            // 1. Adım: Student tablosundaki ismi güncelle
            student.StudentName = model.StudentName;
            await _repository.UpdateUserAsync(student);

            // 2. Adım: User tablosundaki FullName ve şifre alanlarını güncelle
            user.FullName = model.StudentName; // Register'daki FullName mantığına eşitledik
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = newPassword;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Hesap ayarlarınız başarıyla güncellendi.";
            return RedirectToAction("Settings");
        }

        // 3. Hesabı Tamamen Silen Metot (POST)
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteAccount(int id)
{
    var email = Request.Cookies["ActiveUserEmail"];
    if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");
    
    // Silinecek öğrenciyi ve ana kullanıcıyı bul
    var student = await _repository.GetUserByEmailAsync(email);
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    
    // !!! id == student.Id yerine student.StudentId yaptık !!!
    if (student == null || student.StudentId != id || user == null) return BadRequest();

    // 1. Önce Student tablosundan uçuruyoruz
    await _repository.DeleteUserAsync(id);

    // 2. Sonra ana giriş tablosu olan Users'tan siliyoruz
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    // Çerezi temizle ve sistemden güvenle çıkart
    Response.Cookies.Delete("ActiveUserEmail");

    return RedirectToAction("Index", "Home");
}}
}