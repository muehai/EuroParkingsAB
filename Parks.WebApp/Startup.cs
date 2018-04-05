using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Parks.WebApp.Abstact;
using Parks.WebApp.Data;
using Parks.WebApp.Repositories;



namespace Parks.WebApp
{
    public class Startup
    {
       
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //Set up configuration
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VehicleDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("VehiclesConnection")));

            // Repositories
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();

            //Enable core
            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, VehicleDbContext dbContextInitializer)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<VehicleDbContext>().Database.Migrate();
                }
            }
            catch 
            {
               
            }

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Vehicles/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Vehicles}/{action=HomePage}/{id?}");
            });

            //Comment out this function to initialize the database first time 
            //VehiclesDbContextInitializer.Initializer(dbContextInitializer);

        }
    }
}
