using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcE_Learning.Data;
using MvcE_Learning.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));
builder.Services.AddDbContext<EnrollDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));
builder.Services.AddDbContext<InstructorsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstructorDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));
builder.Services.AddDbContext<TeachDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstructorDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));
builder.Services.AddDbContext<ContentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContentDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));
builder.Services.AddDbContext<RatingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDbContext") ?? throw new InvalidOperationException("Connection string 'MvcE_LearningContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
