using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//2-Added
using SingalRAngular1.HubConfig;
using Microsoft.EntityFrameworkCore;
using SingalRAngular1.EFModels;


namespace SingalRAngular1
{
    public class Startup
    {   
        
        //2-Added
        public IConfiguration Configuration { get; }
        //2-Added
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //2-added
            //add your Database Context Pool, and Link Configuration to your Database Connection String 
            //which located at appsettings.json
            services.AddDbContextPool<SignalrContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("MyConnection"))
                );
            //1-added
            //impose policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            //1-added
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            //1-added
            services.AddControllers();

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //1-added //part 1 4.50
            //Apply the policy that configure
            app.UseCors("AllowAllHeaders");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //1-added
                //Map "MyHub" Class, it provides a point talk to end point
                endpoints.MapHub<MyHub>("/toastr"); 
            });
        }
    }
}
