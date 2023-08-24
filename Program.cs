using LowPrice.Models;

namespace LowPrice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dir = Directory.GetDirectories(Environment.CurrentDirectory);
            var check = false;
            foreach (var filename in dir)
            {
                if (filename == Environment.CurrentDirectory + @"\AllItems")
                    check = true; break;
            }

            if (!check)
                CreateDirectory();

            Auth.Connect(@"server=127.0.0.1;uid=root;pwd=1590;database=lowprice");

            var builder = WebApplication.CreateBuilder(args);

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
        }

        private static void CreateDirectory()
        {
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\AllItems\");
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\AllItems\Cybersport");
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\AllItems\Computer");
        }
    }
}