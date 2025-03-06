
using LIB.API.Domain;
using LIB.API.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Persistence
{
    public class LIBAPIDbSQLContext : DbContext
    {
        public readonly DbContextOptions<LIBAPIDbSQLContext> _context;


        public LIBAPIDbSQLContext(DbContextOptions<LIBAPIDbSQLContext> options) : base(options)
        {

            _context = options;

        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LIBAPIDbSQLContext).Assembly);



        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<BaseDomainEntity>())
            {
                // Perform some action for each entity being tracked by the DbContext


            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



      
        public DbSet<Users> Users { get; set; }

 

        public DbSet<InRtgsAtss> InRtgsAtss { get; set; }

        public DbSet<OutRtgsCbcs> OutRtgsCbcs { get; set; }

        public DbSet<InRtgsCbcs> InRtgsCbcs { get; set; }
        public DbSet<OutReconcileds> outReconcileds { get; set; }
        public DbSet<InReconcileds> InReconcileds { get; set; }
        public DbSet<OutRtgsAtss> OutRtgsAtss { get; set; }

    }

}
