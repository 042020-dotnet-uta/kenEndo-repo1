using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Project1.Domain.IRepositories;

namespace Project1
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>
            {
                options.Cookie.Name = "CookieMonster";
                options.LoginPath = "/Welcome/Login";
            });


            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();

            services.AddDbContext<Project1Context>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Project1Context")));

            services.AddScoped<Domain.IRepositories.IRepoUserOrder, Data.Repositories.RepoUserOrder>();
            services.AddScoped<Domain.IRepositories.IRepoStoreLocation, Data.Repositories.RepoStoreLocation>();
            services.AddScoped<Domain.IRepositories.IRepoUserOrderItem, Data.Repositories.RepoUserOrderItem>();
            services.AddScoped<Domain.IRepositories.IRepoStoreItem, Data.Repositories.RepoStoreItem>();
            services.AddScoped<Domain.IRepositories.IRepoUserInfo, Data.Repositories.RepoUserInfo>();
            services.AddScoped<Services.IServiceHome, Services.ServiceHome>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            //authentication after rounting, asking who the user is.
            app.UseAuthentication();
            //asking if the user is allowed to access
            app.UseAuthorization();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Welcome}/{action=Logout}/{id?}");
            });
        }
    }
}
