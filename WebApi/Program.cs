using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<DataContext>();
builder.Services.AddScoped<IAuthorServices, AuthorService>();
builder.Services.AddScoped<IBookServices, BookService>();
builder.Services.AddScoped<IBorrowRecordServices, BorrowRecordService>();
builder.Services.AddScoped<IMemberServices, MemberService>();

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
app.Run();


