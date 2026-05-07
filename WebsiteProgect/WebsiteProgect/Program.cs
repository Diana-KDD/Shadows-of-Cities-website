using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Data;

using (AppDbContext context = new())
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Run();
