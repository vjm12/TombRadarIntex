using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using UserManagement.MVC.Data;
using UserManagement.MVC.Models;
using UserManagement.MVC.Views.Components;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace UserManagement.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<ApplicationDbContext>(options =>
             //    options.UseSqlServer(
             //        Configuration.GetConnectionString("DefaultConnection")));
             //services.AddDatabaseDeveloperPageExceptionFilter();
             services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddControllersWithViews();
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //        .AddEntityFrameworkStores<ApplicationDbContext>()
            //        .AddDefaultUI()
            //.AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //for repository pattern of fag database
            services.AddScoped<IFagElGamousRepository, EFFagELGamousRepository>();
            
            //connect to postgresconnection
            services.AddDbContext<fagContext>(options => {
                options.UseNpgsql(Configuration["ConnectionStrings:PostgresConnection"]);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.ConsentCookie.Expiration = TimeSpan.FromMinutes(5);
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 14;
                options.Password.RequiredUniqueChars = 2;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                //context.Response.Headers.Add("Content-Security-Policy", 
                //    "default-src 'self'; script-src 'self' https://code.jquery.com/jquery-3.4.1.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js " +
                //    "'unsafe-inline';" +
                //    "style-src 'self' https://fonts.googleapis.com/css2?family=Open+Sans:wght@400;500&family=Roboto:wght@500;700;900&display=swap https://fonts.gstatic.com https://fonts.googleapis.com  https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css; " +
                //    "font-src 'self'; " +
                //    "img-src 'self'; frame-src 'self'");

                //                context.Response.Headers.Add("Content-Security-Policy",
                //  "default-src 'self'; " +
                //  "script-src 'self' https://code.jquery.com/jquery-3.4.1.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js 'unsafe-inline'; " +
                //  "style-src 'self' https://fonts.googleapis.com/css2?family=Open+Sans:wght@400 'unsafe-inline'; " +
                //  "font-src 'self' https://fonts.gstatic.com https://fonts.googleapis.com https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css; " +
                //  "img-src 'self'; " +
                //  "frame-src 'self'; " +
                //  "object-src 'none'; " +
                //  "base-uri 'self';"
                //);
                context.Response.Headers.Add("Content-Security-Policy",
                    "default-src 'self'; " +
                    "script-src 'self' https://code.jquery.com/jquery-3.4.1.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js " +
                    "'unsafe-inline'; " +
                    "style-src 'self' https://fonts.googleapis.com/css2?family=Open+Sans:wght@400 'unsafe-inline'; " +
                    "style-src-elem 'self' https://fonts.googleapis.com/css2?family=Open+Sans:wght@400 https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css 'unsafe-inline'; " +
                    "font-src 'self' https://fonts.gstatic.com https://fonts.googleapis.com https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.0/css/all.min.css https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css; " +
                    "img-src 'self'; " +
                    "frame-src 'self';"
                   );
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"typepage","{burialdirec}/Page{pageNum}", new { Controller = "Home", action = "Summary" });
                endpoints.MapControllerRoute( "paging", "Page{pageNum}",new {Controller = "Home", action = "Summary", pageNum = 1}); 
                endpoints.MapControllerRoute("direc", "burialdirec", new { Controller = "Home", action = "Summary", pageNum=1 });
               
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
