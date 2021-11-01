using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ResumeReview.Models;

namespace ResumeReview.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resume> Resume { get; set; }

        public DbSet<Reviews> Reviews { get; set; }

        public DbSet<UserViewed> UserViewed { get; set; }
    }
}
