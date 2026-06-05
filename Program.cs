using Microsoft.EntityFrameworkCore;
using AspKutuphane.Data;
using AspKutuphane.Repositories; // <-- BU SATIRI EKLEMEN GEREKİYOR

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Servisini Kaydet
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=library.db"));

// 2. Repository Servisini Kaydet (Kitap işlemleri için)
builder.Services.AddScoped<BookRepository>();

// 3. MVC Servislerini Kaydet
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();