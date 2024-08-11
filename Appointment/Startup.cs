using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Repositories;
using Appointment.Service;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<SMTPConfigViewModel>(Configuration.GetSection("SMTPConfig"));

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //      .AddEntityFrameworkStores<DataContext>()
            //      .AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>                  /*added*/
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(6);
                options.Lockout.MaxFailedAccessAttempts = 3;

                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = true;
            })
                .AddDefaultUI()                                                          /*added*/
                .AddDefaultTokenProviders()                                              /*added*/
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();                      /*AddRazorRuntimeCompilation*/

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSession(options =>
               {
                   //Session settings
                   options.IdleTimeout = TimeSpan.FromMinutes(30);
               });

            //using package DinkToPdf
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<INeighborhoodRepository, NeighborhoodRepository>();
            services.AddTransient<IUserTypeReository, UserTypeReository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IBranch_HistoryRepository, Branch_HistoryRepository>();
            services.AddTransient<IBranch_NeighborhoodRepository, Branch_NeighborhoodRepository>();
            services.AddTransient<ICareTypeRepository, CareTypeRepository>();
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IDriverRepository, DriverRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IStatisticsRepository, StatisticsRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISessionExtensions, SessionExtensions>();
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
                app.UseStatusCodePagesWithRedirects("/Emplooye/Home/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{Area=Emplooye}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
