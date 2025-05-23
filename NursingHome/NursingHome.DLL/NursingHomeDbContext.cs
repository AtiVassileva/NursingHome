﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<MonthlyParameter> MonthlyParameters { get; set; } = null!;
        public DbSet<StayRate> StayRates { get; set; } = null!;
        public DbSet<DietRate> DietRates { get; set; } = null!;
        public DbSet<MonthlyFee> MonthlyFees { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<SocialDocument> SocialDocuments { get; set; } = null!;
        public DbSet<WeeklyMenu> WeeklyMenus { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<WorkSchedule> WorkSchedules { get; set; } = null!;
        public DbSet<RegulatoryDocument> RegulatoryDocuments { get; set; } = null!;
        public DbSet<RoomPlan> RoomPlans { get; set; } = null!;

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

            builder.Entity<StayRate>()
                .HasOne(sr => sr.Month)
                .WithMany(mp => mp.StayRates)
                .HasForeignKey(sr => sr.MonthId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DietRate>()
                .HasOne(dr => dr.Month)
                .WithMany(mp => mp.DietRates)
                .HasForeignKey(dr => dr.MonthId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MonthlyFee>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MonthlyFee>()
                .HasIndex(f => new { f.UserId, f.Month, f.Year })
                .IsUnique();

            builder.Entity<MonthlyFee>()
                .HasOne(mf => mf.Payment)
                .WithOne(p => p.MonthlyFee)
                .HasForeignKey<Payment>(p => p.MonthlyFeeId);

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithOne(u => u.MedicalRecord)
                .HasForeignKey<MedicalRecord>(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Report>()
                .HasOne(r => r.UploadedBy)
                .WithMany()
                .HasForeignKey(r => r.UploadedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SocialDocument>()
                .HasOne(d => d.Resident)
                .WithMany()
                .HasForeignKey(d => d.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SocialDocument>()
                .HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WeeklyMenu>()
                .HasOne(w => w.UploadedBy)
                .WithMany()
                .HasForeignKey(w => w.UploadedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RegulatoryDocument>()
                .HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RoomPlan>()
                .HasOne(r => r.UploadedBy)
                .WithMany()
                .HasForeignKey(r => r.UploadedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}