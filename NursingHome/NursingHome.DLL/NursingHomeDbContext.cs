using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NursingHome.DAL.Models;

namespace NursingHome.DAL
{
    public class NursingHomeDbContext : IdentityDbContext<ApplicationUser>
    {
        public NursingHomeDbContext()
        {
        }
        public NursingHomeDbContext(DbContextOptions<NursingHomeDbContext> options)
            : base(options)
        {
        }

        public DbSet<ResidentInfo> ResidentInfos { get; set; } = null!;
        public DbSet<EmployeeInfo> EmployeeInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=NursingHome;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ResidentInfo>()
                .HasOne(r => r.User)
                .WithOne(u => u.ResidentInfo)
                .HasForeignKey<ResidentInfo>(r => r.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<EmployeeInfo>()
                .HasOne(e => e.User)
                .WithOne(u => u.EmployeeInfo)
                .HasForeignKey<EmployeeInfo>(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ResidentInfo>()
                .Property(r => r.Pension)
                .HasColumnType("decimal(18,2)");

            builder.Entity<ResidentInfo>()
                .Property(r => r.Rent)
                .HasColumnType("decimal(18,2)");

            builder.Entity<ResidentInfo>()
                .Property(r => r.Salary)
                .HasColumnType("decimal(18,2)");

            builder.Entity<ResidentInfo>()
                .Property(r => r.OtherIncome)
                .HasColumnType("decimal(18,2)");
        }
    }
}