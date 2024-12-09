using DotnetApiPostgres.Api;
using DotnetApiPostgres.Api.Models;
using DotnetApiPostgres.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.WebHost.UseUrls("http://0.0.0.0:80");

// Зчитуємо рядок підключення з конфігураційного файлу
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Налаштування контексту БД для підключення до PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Регістрація сервісу для роботи з особами
builder.Services.AddTransient<IPersonService, PersonService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Додавання маршруту для кореневої сторінки
app.MapGet("/", () => "Welcome to DotnetApiPostgres API! Now with a new message! Here new message that i added");

app.Run();

