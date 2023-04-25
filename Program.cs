var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

// 先試著連線到資料庫

app.Run();
