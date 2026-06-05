using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspKutuphane.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    StudentMail = table.Column<string>(type: "TEXT", nullable: false),
                    Department = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    IsBorrowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Image", "IsBorrowed", "StudentId", "Title" },
                values: new object[,]
                {
                    { 1, "1.jpg.webp", false, null, "Bozkırın Oğlu" },
                    { 2, "2.jpg.webp", false, null, "Bozkırın İlk İmparatorluğu Hunlar" },
                    { 3, "3.jpg.webp", false, null, "Bozkırın Kağanlıkları" },
                    { 4, "4.jpg.webp", false, null, "Gök Tengrinin Çocukları" },
                    { 5, "5.jpg.webp", false, null, "Eski Türk Tarihi" },
                    { 6, "6.jpg.webp", false, null, "Gökbörünün İzinde" },
                    { 7, "7.jpg.webp", false, null, "Eski Türk Boyları" },
                    { 8, "8.jpg.webp", false, null, "Tarih Boyunca Türk Kadını" },
                    { 9, "9.jpg.webp", false, null, "İlk Türkler" },
                    { 10, "10.jpg.webp", false, null, "Kadim Türkler" },
                    { 11, "11.jpg.webp", false, null, "İslamiyet Öncesi Türkler" },
                    { 12, "12.jpg.webp", false, null, "Milli Mücadele Tarihi" },
                    { 13, "13.jpg.webp", false, null, "Osmanlı Ve Avrupa" },
                    { 14, "14.jpg.webp", false, null, "Devlet-i Alliyye" },
                    { 15, "15.jpg.webp", false, null, "Osmanlı Tarihinde Efsaneler ve Gerçekler" },
                    { 16, "16.jpg.webp", false, null, "İmparatorluktan Cumhuriyete" },
                    { 17, "17.jpg.webp", false, null, "Kısa Osmanlı Tarihi" },
                    { 18, "18.jpg.webp", false, null, "Fatih Sultan Mehmet Han" },
                    { 19, "19.jpg.webp", false, null, "Osmanlı İmparatorluğu Klasik Çağ" },
                    { 20, "20.jpg.webp", false, null, "Cumhiriyetin İlk Yüzyılı" },
                    { 21, "21.jpg.webp", false, null, "İmparatorluğun En Uzun Yüzyılı" },
                    { 22, "22.jpg.webp", false, null, "Türkiyenin Yakın Tarihi" },
                    { 23, "23.jpg.webp", false, null, "Türklerin Tarihi" },
                    { 24, "24.jpg.webp", false, null, "Kısa Osmanlı Tarihi" },
                    { 25, "25.jpg.webp", false, null, "Yakın Tarihin Gerçekleri" },
                    { 26, "26.jpg.webp", false, null, "İlber Ortaylı Seyahatnamesi" },
                    { 27, "27.jpg.webp", false, null, "Kurtuluş Cumhuriyete Giden Yol" },
                    { 28, "28.jpg.webp", false, null, "Zaman Kaybolmaz" },
                    { 29, "29.jpg.webp", false, null, "Gel Dünyayı Keşfetelim" },
                    { 30, "30.jpg.webp", false, null, "Gazi Mustafa Kemal Atatürk" },
                    { 31, "31.jpg.webp", false, null, "Fatih Sultan Mehmet Han" },
                    { 32, "32.jpg.webp", false, null, "Bir Ömür Nasıl Yaşanır" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_StudentId",
                table: "Books",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
