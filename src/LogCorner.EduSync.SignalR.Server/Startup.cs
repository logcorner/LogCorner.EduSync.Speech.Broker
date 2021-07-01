using LogCorner.EduSync.SignalR.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

namespace LogCorner.EduSync.SignalR.Server
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                var allowedOrigins = Configuration["allowedOrigins"]?.Split(",");
                options.AddPolicy("corsPolicy",
                    builder =>
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins(allowedOrigins)
                            .AllowCredentials()
                    );
            });

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            services.AddSignalR(log =>
            {
                log.EnableDetailedErrors = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("corsPolicy");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("LogCorner Hub Notification Started Successfully !");
                });

                endpoints.MapHub<LogCornerHub<object>>("/logcornerhub").RequireAuthorization();
            });
        }
    }
}