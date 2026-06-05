namespace AspKutuphane.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string? Image { get; set; } 
        public bool IsBorrowed { get; set; } 
        
        // İlişki Kurulumu
        public int? StudentId { get; set; } 
        public Student? Student { get; set; } 
    }
}