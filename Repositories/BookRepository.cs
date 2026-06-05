using AspKutuphane.Data;
using AspKutuphane.Models;
using Microsoft.EntityFrameworkCore;

namespace AspKutuphane.Repositories
{
    public class BookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> BorrowBookAsync(int bookId, string studentEmail)
        {
            var book = await _context.Books.FindAsync(bookId);
            var student = await _context.Students
                .Include(s => s.BorrowedBooks)
                .FirstOrDefaultAsync(s => s.StudentMail == studentEmail);

            if (book != null && student != null && !book.IsBorrowed)
            {
                book.IsBorrowed = true;
                student.BorrowedBooks.Add(book);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Book>> GetBorrowedBooksAsync()
        {
            return await _context.Books
                .Where(b => b.IsBorrowed)
                .Include(b => b.Student) 
                .ToListAsync();
        }

        public async Task<bool> ReturnBookAsync(int bookId)
        {
            var book = await _context.Books
                .Include(b => b.Student)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book != null && book.IsBorrowed)
            {
                book.IsBorrowed = false; 
                book.Student = null;     

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        
        // HESAP AYARLARI İÇİN ÖĞRENCİ (USER) METOTLARI (Sınıf İçine Alındı)
        

        // 1. E-posta adresine göre öğrenci bilgilerini getiren metot
        public async Task<Student?> GetUserByEmailAsync(string email)
        {
            // Projendeki StudentMail alanı ile eşledim
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentMail == email);
        }

        // 2. Öğrenci bilgilerini güncelleyen metot
        public async Task UpdateUserAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        // 3. Öğrenci hesabını tamamen silen metot
        public async Task DeleteUserAsync(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    } // <-- BookRepository sınıfı burada güvenle kapanıyor
} // <-- Namespace burada güvenle kapanıyor