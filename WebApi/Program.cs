using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(opt =>
{
    opt.AccessDeniedPath = new PathString("/api/auth/login");
    opt.Cookie.Name = "MyCookie";
    opt.Cookie.HttpOnly = true; // XSS => Blocks javascript 
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IAuthorServices, AuthorService>();
builder.Services.AddScoped<IBookServices, BookService>();
builder.Services.AddScoped<IBorrowRecordServices, BorrowRecordService>();
builder.Services.AddScoped<IMemberServices, MemberService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(p => p.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));

}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();


