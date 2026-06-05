using Microsoft.AspNetCore.Mvc;
using AspKutuphane.Repositories;
using AspKutuphane.Models;

namespace AspKutuphane.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _repository;

        public BookController(BookRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _repository.GetAllBooksAsync();
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null) return NotFound();

            var book = await _repository.GetBookByIdAsync(id.Value);

            if (book == null || book.IsBorrowed) return RedirectToAction(nameof(Index));

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowConfirm(int id)
        {
            var email = Request.Cookies["ActiveUserEmail"];
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");

            var result = await _repository.BorrowBookAsync(id, email);

            if (result)
            {
                return RedirectToAction("Profile", "Account");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    book.Image = fileName;
                }

                await _repository.AddBookAsync(book);
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public async Task<IActionResult> EmanetTakibi()
        {
            var borrowedBooks = await _repository.GetBorrowedBooksAsync();
            return View(borrowedBooks);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var result = await _repository.ReturnBookAsync(id);

            if (result)
            {
                return RedirectToAction(nameof(EmanetTakibi));
            }

            return BadRequest("İade işlemi sırasında bir hata oluştu.");
        }

       

        // 1. GÜNCELLEME SAYFASINI AÇAN METOT (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Veri tabanından kitabı repository vasıtasıyla çekiyoruz
            var book = await _repository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // 2. GÜNCELLEME İŞLEMİNİ KAYDEDEN METOT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book model, IFormFile? imageFile)
        {
            var book = await _repository.GetBookByIdAsync(model.Id);
            if (book == null) 
            { 
                return NotFound(); 
            }

            // Sadece mevcut alanları güncelliyoruz
            book.Title = model.Title;

            // Yeni resim dosyası seçildiyse işlemleri yürüt
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                book.Image = fileName;
            }

            // Değişiklikleri kaydetmesi için Repository'deki güncelleme metodunu tetikliyoruz
            await _repository.UpdateBookAsync(book);
            
            return RedirectToAction("Index");
        }

        // 3. KİTABI SİLEN METOT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _repository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Repository üzerinden silme metodunu tetikliyoruz
            await _repository.DeleteBookAsync(id);
            
            return RedirectToAction("Index");
        }
    }
}