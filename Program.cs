using SharpRTSPtoWebRTC; // <-- Важная строка!
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем сервис WebRTC-прокси
builder.Services.AddSingleton<RTSPtoWebRTCProxyService>();

// Загружаем конфигурацию камер из appsettings.json и регистрируем как singleton
var cameras = builder.Configuration.GetSection("Cameras").Get<List<CameraConfiguration>>() ?? new List<CameraConfiguration>();
builder.Services.AddSingleton(cameras);

// Добавляем контроллеры и Razor Pages (проект содержит Razor Pages)
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Настраиваем CORS (для тестирования разрешаем все источники)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles(); // Для отдачи index.html по умолчанию
app.UseStaticFiles();  // Для раздачи статических файлов из wwwroot
app.MapControllers();
app.MapRazorPages();

app.Run();
