using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sistema_Nominas.Interfaces;

namespace Sistema_Nominas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
            builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Usar la cadena de conexión (por ejemplo, para configurar DbContext)
            builder.Services.AddSingleton(connectionString);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            // Configura el enrutamiento para archivos .aspx
            app.MapWhen(context => context.Request.Path.Value.EndsWith(".aspx"), builder =>
            {
                builder.UseStaticFiles();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
