namespace AspKutuphane.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentMail { get; set; }
        public string Department { get; set; } = string.Empty;
        public List<Book> BorrowedBooks { get; set; } = new(); 
    }
}