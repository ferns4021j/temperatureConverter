using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace ASPNetApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                    // builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .WithMethods("GET", "OPTIONS", "HEAD");
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Temparature Converter", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("AllowSpecificOrigins");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Temparature Converter v1"));
            }

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                await next.Invoke();

                if (context.Response.StatusCode == 404 && !context.Request.Path.Value.Contains("/api") && 
                !context.Request.Path.Value.Contains("/swagger"))
                {
                    context.Request.Path = new PathString("/index.html");
                    await next.Invoke();
                }
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
