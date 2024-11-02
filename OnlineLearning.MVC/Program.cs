using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineLearning.Entity.Entities;
using OnlineLearning.Infrastructure.Data;
using OnlineLearning.Infrastructure.Repositories;
using OnlineLearning.Service.Interfaces;
using OnlineLearning.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineLearning.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IProgressTrackingService, ProgressTrackingService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount=true*/)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
            app.MapRazorPages();

            // Add the code to seed roles and users
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                SeedUserRoles(userManager, roleManager);
            }

            app.Run();
        }

        private static void SeedUserRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // List of roles to seed
            var roles = new List<string> { "Admin", "Instructor", "Student" };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }

            // Seed Admin User (replace the email with your admin user's email)
            var adminUser = userManager.FindByEmailAsync("hossam.sabeer55@gmail.com").Result;
            if (adminUser != null && !userManager.IsInRoleAsync(adminUser, "Admin").Result)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }

            // Seed Instructor User (replace the email with your instructor user's email)
            var instructorUser = userManager.FindByEmailAsync("Mohamed@gmail.com").Result;
            if (instructorUser != null && !userManager.IsInRoleAsync(instructorUser, "Instructor").Result)
            {
                userManager.AddToRoleAsync(instructorUser, "Instructor").Wait();
            }
        }
    }
}
