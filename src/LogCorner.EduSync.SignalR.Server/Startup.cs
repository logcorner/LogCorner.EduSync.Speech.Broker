using LogCorner.EduSync.SignalR.Server.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(options =>
                    {
                        Configuration.Bind("AzureAdB2C", options);

                        options.TokenValidationParameters.NameClaimType = "name";
                        options.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                return Task.CompletedTask;
                            },
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];
                                var path = context.HttpContext.Request.Path;

                                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/logcornerhub")))
                                {
                                    context.Token = accessToken;
                                }

                                return Task.CompletedTask;
                            },
                            OnAuthenticationFailed = context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                context.Response.ContentType = "application/json";
                                var err = context.Exception.ToString();
                                var result = JsonSerializer.Serialize(new { err });
                                return context.Response.WriteAsync(err);
                            }
                        };
                    },
            options => { Configuration.Bind("AzureAdB2C", options); });

            services.AddSignalR(log =>
            {
                log.EnableDetailedErrors = true;
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("LogCorner Hub Notification Started Successfully !");
                });

                endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");//.RequireAuthorization();
            });
        }
    }
}