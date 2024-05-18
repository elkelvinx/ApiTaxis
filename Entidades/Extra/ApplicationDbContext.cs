using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Extra
{
    internal class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        /*
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
           {

           }

           public DbSet<user> Personas { get; set; }

         */
    }
}
