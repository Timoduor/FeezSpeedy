using FeezSpeedy.Models;
using FeezSpeedy.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FeezSpeedy.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<Parent>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dependant> Dependants { get; set; }
        public DbSet<Parent> Parents { get; set; }  
        public DbSet<School> Schools { get; set; }
        public DbSet<FeeRequest> FeeRequests { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        public DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }
        public DbSet<LoanStatus> LoanStatuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());
            }

            // 👇 Add this to link FeeRequest -> Parent
            builder.Entity<FeeRequest>()
                .HasOne(f => f.Parent)
                .WithMany() // or .WithMany(p => p.FeeRequests) if Parent tracks requests
                .HasForeignKey(f => f.ParentId)
                .IsRequired();
        }
    }
}
