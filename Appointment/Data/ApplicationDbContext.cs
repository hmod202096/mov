using Appointment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           

            // تحديد المفتاح الأساسي لجدول الفرع_التاريخ 
            builder.Entity<Branches_HistoryDates>()
                    .HasKey(bc => new { bc.HistoryDateId, bc.BranchId });
            builder.Entity<Branches_HistoryDates>()
                    .HasOne(bc => bc.Branch)
                    .WithMany(b => b.Branches_HistoryDates)
                    .HasForeignKey(bc => bc.BranchId);
            builder.Entity<Branches_HistoryDates>()
                    .HasOne(bc => bc.HistoryDate)
                    .WithMany(c => c.Branches_HistoryDates)
                    .HasForeignKey(bc => bc.HistoryDateId);

            // تحديد المفتاح الأساسي لجدول الفرع_الاحياء 
            builder.Entity<Branches_Neighborhoodes>()
                    .HasKey(bc => new { bc.NeighborhoodId, bc.BranchId });
            builder.Entity<Branches_Neighborhoodes>()
                    .HasOne(bc => bc.Branches)
                    .WithMany(b => b.Branches_Neighborhoodes)
                    .HasForeignKey(bc => bc.BranchId);
            builder.Entity<Branches_Neighborhoodes>()
                    .HasOne(bc => bc.Neighborhoods)
                    .WithMany(c => c.Branches_Neighborhoodes)
                    .HasForeignKey(bc => bc.NeighborhoodId);
        }

        public DbSet<Neighborhoods> Neighborhoods { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<HistoryDate> HistoryDates { get; set; }
        public DbSet<Branches_HistoryDates> Branches_HistoryDates { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Branches_Neighborhoodes> Branches_Neighborhoodes { get; set; }
        public DbSet<CareTypes> CareTypes { get; set; }
        public DbSet<Cares> Cares { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }

    }
}
