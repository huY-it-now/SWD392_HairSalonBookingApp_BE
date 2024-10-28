using Azure;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<ComboService> ComboServices { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<SalonMember> SalonMembers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PaymentLogs> PaymentLogs { get; set; }
        public DbSet<PaymentMethods> PaymentMethods { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<PaymentStatus> PaymentSatus { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<SalonMemberSchedule> SalonMemberSchedules { get; set; }
        public DbSet<ServiceComboService> ServiceComboServices { get; set; }
        public DbSet<ComboServiceComboDetail> ComboServiceComboDetails { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ScheduleWorkTime> ScheduleWorkTime { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleDetail = "Admin" },
                new Role { Id = 2, RoleDetail = "Customer" },
                new Role { Id = 3, RoleDetail = "Salon Manager" },
                new Role { Id = 4, RoleDetail = "Salon Staff" },
                new Role { Id = 5, RoleDetail = "Stylist" }
            );

            modelBuilder.Entity<Service>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Category_Service");

            modelBuilder.Entity<SalonMember>()
                .HasOne(c => c.Salon)
                .WithMany(c => c.SalonMembers)
                .HasForeignKey(s => s.SalonId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_SalonMember_Salon");

            modelBuilder.Entity<User>()
                .HasOne(u => u.SalonMember)
                .WithOne(sm => sm.User)
                .HasForeignKey<SalonMember>(sm => sm.Id)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_User_SalonMember");

            modelBuilder.Entity<SalonMember>()
                .HasOne(sm => sm.User)
                .WithOne(u => u.SalonMember)
                .HasForeignKey<SalonMember>(sm => sm.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_SalonMember_User");

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.SalonMembers)
                .WithOne(s => s.Salon)
                .HasForeignKey(s => s.SalonId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Stylist)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.StylistId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<ScheduleWorkTime>()
                .HasOne(swt => swt.SalonMemberSchedule)
                .WithMany(sms => sms.WorkTime)
                .HasForeignKey(swt => swt.SalonMemberScheduleId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.ComboService).WithMany(c => c.Appointments).HasForeignKey(s => s.ComboServiceId);

            // Áp dụng bộ lọc toàn cục cho Service
            modelBuilder.Entity<Service>().HasQueryFilter(c => !c.IsDeleted);
            // Áp dụng bộ lọc toàn cục cho Category
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceComboService>()
                .HasKey(x => new { x.ServiceId, x.ComboServiceId });

            modelBuilder.Entity<ComboServiceComboDetail>()
                .HasKey(x => new { x.ComboServiceId, x.ComboDetailId });
        }

    }
}
