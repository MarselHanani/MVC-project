using Booky.ADL.Data;
using Booky.ADL.Models;
using Booky.BL.Interface;
using Booky.BL.Repository;
using Booky.PL.MappingProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BookyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));




//========================================================================================================

builder.Services.AddAutoMapper(c => c.AddProfile<ProductProfile>());

//========================================================================================================




builder.Services.AddScoped(typeof(IunitOfWork), typeof(UnitOfWork));
builder.Services.AddSingleton<IHostingEnvironment>(env => new HostingEnvironment()); 
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
    pattern: "{area=Customer}/{controller=Home}/{action=Index}");

app.Run();