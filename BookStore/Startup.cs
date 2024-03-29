﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

                var builder = new DbContextOptionsBuilder<AppDbContext>();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                builder.UseSqlServer(connectionString);


                return new AppDbContext(builder.Options);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = 

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Configure the Identity manager and user manager for the app
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<IdentityRole>>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var serviceProvider = app.ApplicationServices;

            using (var scope = serviceProvider.CreateScope())
            {
                //Create admin role
                CreateRoles(serviceProvider).Wait();

                //Add the administrator role to the admin account
                AddAdminToRole(serviceProvider).Wait();
            }


            app.UseHttpsRedirection();
            app.UseIdentity();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Books}/{action=Index}/{id?}");
            });
        }

        //Create an admin role using the role manager
        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var adminRoleExists = roleManager.RoleExistsAsync("Administrator");
                if (!adminRoleExists.Result)
                {
                    await roleManager.CreateAsync(new IdentityRole("Administrator"));
                }
            }
        }

        //Add the admin role to the administrator account
        public async Task AddAdminToRole(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var admin = await userManager.FindByNameAsync("lgosh@gmail.com");
                
                await userManager.AddToRoleAsync(admin, "Administrator");
            }
        }
    }
}
