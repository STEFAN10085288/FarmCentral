using FarmCentral.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FarmCentral
{
    public class Program
    {
        //public static void Main
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
            });

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            //makes roles on startup
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Employee", "Farmer" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            //assign roles to users
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string email1 = "stefan@gmail.com";
                string password1 = "123456";

                string email2 = "clayton@gmail.com";
                string password2 = "123456";

                string email3 = "dylan@gmail.com";
                string password3 = "123456";

                if (await userManager.FindByEmailAsync(email1) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email1;
                    user.Email = email1;

                    await userManager.CreateAsync(user,password1);

                    await userManager.AddToRoleAsync(user,"Employee");
                }

                if (await userManager.FindByEmailAsync(email2) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email2;
                    user.Email = email2;

                    await userManager.CreateAsync(user, password2);

                    await userManager.AddToRoleAsync(user, "Farmer");
                }

                if (await userManager.FindByEmailAsync(email3) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email3;
                    user.Email = email3;

                    await userManager.CreateAsync(user, password3);

                    await userManager.AddToRoleAsync(user, "Farmer");
                }
            }

            app.Run();
        }
    }
}