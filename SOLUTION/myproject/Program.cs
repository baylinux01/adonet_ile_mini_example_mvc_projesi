using myproject.Repositories;
using myproject.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DatabaseRepository>();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<AppUserRepository>();
builder.Services.AddScoped<AppUserService>();
 builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

var app = builder.Build();
app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
    try
    {
        dbService.CreateDatabase();
    }
    catch(Exception ex1)
    {
        
    }
    finally
    {
        
    }
    try
    {
       dbService.CreateAppUsersTable(); 
    }
     catch(Exception ex2)
    {
        
    }
    finally
    {
        
    }
    
    
}


    

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
