
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
    public class LIBAPIDbContext : DbContext
    {
        public readonly DbContextOptions<LIBAPIDbContext> _context;


        public LIBAPIDbContext(DbContextOptions<LIBAPIDbContext> options) : base(options)
        {

            _context = options;

        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LIBAPIDbContext).Assembly);

            modelBuilder.Entity<InRtgsCbcsOracle>()
        .Property(e => e.AMOUNT)  // Ensure property name matches
        .HasPrecision(18, 2);     // Adjust precision and scale as needed

            modelBuilder.Entity<OutRtgsCbcsOracle>()
                .Property(e => e.AMOUNT)
                .HasPrecision(18, 2);

        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<BaseDomainEntity>())
            {
                // Perform some action for each entity being tracked by the DbContext


            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }




        public DbSet<InRtgsAtssOracle> InRtgsAtssOracle { get; set; }

        public DbSet<OutRtgsCbcsOracle> OutRtgsCbcsOracle { get; set; }

        public DbSet<InRtgsCbcsOracle> InRtgsCbcsOracle { get; set; }
        public DbSet<OutRtgsAtssOracle> OutRtgsAtssOracle { get; set; }


        public DbSet<AccountInfos> AccountInfos { get; set; }

        public DbSet<UserData> userDatas { get; set; }
    }

}
