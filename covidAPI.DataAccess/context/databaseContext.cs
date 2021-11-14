using System;
using covidAPI.model;
using Microsoft.EntityFrameworkCore;

namespace covidAPI.DataAccess.context
{
    public class databaseContext : DbContext
    {
        public databaseContext(DbContextOptions<databaseContext> options) : base(options) { }

        public DbSet<user> users { get; set; }
        public DbSet<risk_trkr> risk_Trkrs { get; set; }
        public DbSet<symptom> symptoms { get; set; }
        public DbSet<zone> zones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>(entity => {
                entity.ToTable("user").HasKey(K => K.user_id);
            });
            modelBuilder.Entity<risk_trkr>(entity => {
                entity.ToTable("risk_trkr").HasKey(K => K.u_id);
            });
            modelBuilder.Entity<symptom>(entity => {
                entity.ToTable("symptom").HasKey(K => K.s_id);
            });
            modelBuilder.Entity<zone>(entity => {
                entity.ToTable("zone").HasKey(K => K.zone_id);
            });

        }
    }
}
