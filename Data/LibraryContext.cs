using Microsoft.EntityFrameworkCore;
using AspKutuphane.Models;

namespace AspKutuphane.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        // Migration araçları için parametresiz yapıcı metot
        public LibraryContext() { }

        // TABLOLAR BURADA TANIMLANIR
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<User> Users { get; set; } // Kullanıcılar tablosunu buraya aldık

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=library.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Önemli: Base metodu çağırıyoruz

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Bozkırın Oğlu", IsBorrowed = false, Image = "1.jpg.webp" },
                new Book { Id = 2, Title = "Bozkırın İlk İmparatorluğu Hunlar", IsBorrowed = false, Image = "2.jpg.webp" },
                new Book { Id = 3, Title = "Bozkırın Kağanlıkları", IsBorrowed = false, Image = "3.jpg.webp" },
                new Book { Id = 4, Title = "Gök Tengrinin Çocukları", IsBorrowed = false, Image = "4.jpg.webp" },
                new Book { Id = 5, Title = "Eski Türk Tarihi", IsBorrowed = false, Image = "5.jpg.webp" },
                new Book { Id = 6, Title = "Gökbörünün İzinde", IsBorrowed = false, Image = "6.jpg.webp" },
                new Book { Id = 7, Title = "Eski Türk Boyları", IsBorrowed = false, Image = "7.jpg.webp" },
                new Book { Id = 8, Title = "Tarih Boyunca Türk Kadını", IsBorrowed = false, Image = "8.jpg.webp" },
                new Book { Id = 9, Title = "İlk Türkler", IsBorrowed = false, Image = "9.jpg.webp" },
                new Book { Id = 10, Title = "Kadim Türkler", IsBorrowed = false, Image = "10.jpg.webp" },
                new Book { Id = 11, Title = "İslamiyet Öncesi Türkler", IsBorrowed = false, Image = "11.jpg.webp" },
                new Book { Id = 12, Title = "Milli Mücadele Tarihi", IsBorrowed = false, Image = "12.jpg.webp" },
                new Book { Id = 13, Title = "Osmanlı Ve Avrupa", IsBorrowed = false, Image = "13.jpg.webp" },
                new Book { Id = 14, Title = "Devlet-i Alliyye", IsBorrowed = false, Image = "14.jpg.webp" },
                new Book { Id = 15, Title = "Osmanlı Tarihinde Efsaneler ve Gerçekler", IsBorrowed = false, Image = "15.jpg.webp" },
                new Book { Id = 16, Title = "İmparatorluktan Cumhuriyete", IsBorrowed = false, Image = "16.jpg.webp" },
                new Book { Id = 17, Title = "Kısa Osmanlı Tarihi", IsBorrowed = false, Image = "17.jpg.webp" },
                new Book { Id = 18, Title = "Fatih Sultan Mehmet Han", IsBorrowed = false, Image = "18.jpg.webp" },
                new Book { Id = 19, Title = "Osmanlı İmparatorluğu Klasik Çağ", IsBorrowed = false, Image = "19.jpg.webp" },
                new Book { Id = 20, Title = "Cumhiriyetin İlk Yüzyılı", IsBorrowed = false, Image = "20.jpg.webp" },
                new Book { Id = 21, Title = "İmparatorluğun En Uzun Yüzyılı", IsBorrowed = false, Image = "21.jpg.webp" },
                new Book { Id = 22, Title = "Türkiyenin Yakın Tarihi", IsBorrowed = false, Image = "22.jpg.webp" },
                new Book { Id = 23, Title = "Türklerin Tarihi", IsBorrowed = false, Image = "23.jpg.webp" },
                new Book { Id = 24, Title = "Kısa Osmanlı Tarihi", IsBorrowed = false, Image = "24.jpg.webp" },
                new Book { Id = 25, Title = "Yakın Tarihin Gerçekleri", IsBorrowed = false, Image = "25.jpg.webp" },
                new Book { Id = 26, Title = "İlber Ortaylı Seyahatnamesi", IsBorrowed = false, Image = "26.jpg.webp" },
                new Book { Id = 27, Title = "Kurtuluş Cumhuriyete Giden Yol", IsBorrowed = false, Image = "27.jpg.webp" },
                new Book { Id = 28, Title = "Zaman Kaybolmaz", IsBorrowed = false, Image = "28.jpg.webp" },
                new Book { Id = 29, Title = "Gel Dünyayı Keşfetelim", IsBorrowed = false, Image = "29.jpg.webp" },
                new Book { Id = 30, Title = "Gazi Mustafa Kemal Atatürk", IsBorrowed = false, Image = "30.jpg.webp" },
                new Book { Id = 31, Title = "Fatih Sultan Mehmet Han", IsBorrowed = false, Image = "31.jpg.webp" },
                new Book { Id = 32, Title = "Bir Ömür Nasıl Yaşanır", IsBorrowed = false, Image = "32.jpg.webp" }
            );
        } // Metot burada düzgünce kapandı
    }
}