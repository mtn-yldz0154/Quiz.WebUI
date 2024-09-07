using Microsoft.EntityFrameworkCore;
using Quiz.WebUI;
using Quiz.WebUI.Context;
using Quiz.WebUI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SignalR'� ekliyoruz
builder.Services.AddSignalR();
builder.Services.AddHostedService<MyBackgroundService>();

// Veritaban� ba�lant�s�n� ekliyoruz
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route ayar�
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

// SignalR Hub endpoint'ini burada tan�ml�yoruz
app.MapHub<QuizHub>("/quizHub");  // "/quizHub" SignalR i�in URL'yi temsil ediyor

app.Run();
