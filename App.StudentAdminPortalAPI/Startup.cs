using App.StudentAdminPortalAPI.DataModels;
using App.StudentAdminPortalAPI.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
namespace App.StudentAdminPortalAPI
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
            services.AddCors((options) =>
            {
                options.AddPolicy("AngularApplication", (builder) =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .WithMethods("GET", "POST", "PUT", "DELETE")
                           .WithExposedHeaders("*");
                });
            });

            services.AddControllers();

            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddDbContext<StudentAdminContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StudentAdminPortalDb")));

            services.AddScoped<IStudentRepository, SqlStudentRepository>();
            services.AddScoped<IImageRepository, LocalStorageImageRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "App.StudentAdminPortalAPI",
                    Description = "A simple ASP.NET Core web API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "DungCW",
                        Email = "dungkbkbhn@gmail.com"
                    }
                });
            });

            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App.StudentAdminPortalAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources")),
                RequestPath = "/Resources"
            });

            app.UseRouting();

            app.UseCors("AngularApplication");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}