using System;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeReview.Areas.Identity.Data;
using ResumeReview.Data;

[assembly: HostingStartup(typeof(ResumeReview.Areas.Identity.IdentityHostingStartup))]
namespace ResumeReview.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                //services.AddDbContext<ResumeReviewDbContext>(options =>
                //    options.UseSqlServer(
                //        context.Configuration.GetConnectionString("ResumeReviewDbContextConnection")));

                services.AddDbContextPool<ApplicationDbContext>(options =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    string connStr;

                    // Depending on if in development or production, use either Heroku-provided
                    // connection string, or development connection string from env var.
                    if (env == "Development")
                    {
                        connStr = context.Configuration.GetConnectionString("DevelopmentConnection");
                        options.UseSqlServer(connStr);
                    }
                    else
                    {
                        // Use connection string provided at runtime by Heroku.
                        //var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                        var connUrl = context.Configuration.GetConnectionString("ProductionConnection");
                        // Parse connection URL to connection string for Npgsql
                        connUrl = connUrl.Replace("postgres://", string.Empty);
                        var pgUserPass = connUrl.Split("@")[0];
                        var pgHostPortDb = connUrl.Split("@")[1];
                        var pgHostPort = pgHostPortDb.Split("/")[0];
                        var pgDb = pgHostPortDb.Split("/")[1];
                        var pgUser = pgUserPass.Split(":")[0];
                        var pgPass = pgUserPass.Split(":")[1];
                        var pgHost = pgHostPort.Split(":")[0];
                        var pgPort = pgHostPort.Split(":")[1];

                        connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};sslmode=Require;Trust Server Certificate=true;";
                        options.UseNpgsql(connStr);
                    }
                });

                services.AddDefaultIdentity<ApplicationUser>(options => {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.SignIn.RequireConfirmedAccount = true;
                })
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            });
        }
    }
}