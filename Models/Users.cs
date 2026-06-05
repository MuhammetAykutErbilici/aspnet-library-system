namespace AspKutuphane.Models
{
    public class User
    {
        public int Id { get; set; } // Otomatik Id
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Gerçek projelerde şifrelenmelidir
        public string Role { get; set; } = "User"; // Varsayılan rol
    }
}