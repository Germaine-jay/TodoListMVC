using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoList.BLL.Implementations;
using TodoList.BLL.Interfaces;
using TodoList.DAL.Database;
using TodoList.DAL.Repository;

class program {
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<TodoListDbContext>(opts =>
        {
            // this will only work if there's a section called ConnectionStrings on the appSettings
            // var defaultConn = builder.Configuration.GetConnectionString("DefaultConn");

            var defaultConn = builder.Configuration.GetSection("ConnectionString")["DefaultConn"];
            opts.UseSqlServer(defaultConn);

        });

        builder.Services.AddControllersWithViews();
        // Add services to the container.

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork<TodoListDbContext>>();
        builder.Services.AddScoped<IUserService, UserService>();//todo: show other life-cycles
        builder.Services.AddScoped<ITodoListService, TodoListService>();//todo: show other life-cycles
        builder.Services.AddAutoMapper(Assembly.Load("TodoList.BLL"));
       

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


        await DataAccess.EnsurePopulatedAsync(app);

        await app.RunAsync();

    }

}
