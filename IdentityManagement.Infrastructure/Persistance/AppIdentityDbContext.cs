using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManagement.Infrastructure.Persistance
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>()
                .HasData(new IdentityRole 
                {
                    Name="user" 
                });
            base.OnModelCreating(builder);
        }
    }
}
