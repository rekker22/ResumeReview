using System;
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
                services.AddDbContext<ResumeReviewDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ResumeReviewDbContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.SignIn.RequireConfirmedAccount = true;
                })
                    .AddEntityFrameworkStores<ResumeReviewDbContext>();
            });
        }
    }
}