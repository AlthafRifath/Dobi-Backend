using Dobi.Application.Abstractions.Persistence;
using Dobi.Domain.Auditing;
using Dobi.Domain.Branches;
using Dobi.Domain.Collections;
using Dobi.Domain.Customers;
using Dobi.Domain.Identity;
using Dobi.Domain.Inspections;
using Dobi.Domain.Notifications;
using Dobi.Domain.Orders;
using Dobi.Domain.Payments;
using Dobi.Domain.PlantProcessing;
using Dobi.Domain.Plants;
using Dobi.Domain.Services;
using Dobi.Domain.Transfers;
using Dobi.Infrastructure.Identity;
using Dobi.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence
{
    public class DobiDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IDobiDbContext
    {
        public DobiDbContext(DbContextOptions<DobiDbContext> options)
            : base(options)
        {
        }

        // Branches / Plants
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Plant> Plants => Set<Plant>();

        // Dobi-specific user assignments
        public DbSet<UserBranchAssignment> UserBranchAssignments => Set<UserBranchAssignment>();
        public DbSet<UserPlantAssignment> UserPlantAssignments => Set<UserPlantAssignment>();

        // Customers
        public DbSet<CustomerType> CustomerTypes => Set<CustomerType>();
        public DbSet<Customer> Customers => Set<Customer>();

        // Services / Pricing
        public DbSet<PricingType> PricingTypes => Set<PricingType>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();
        public DbSet<ServicePrice> ServicePrices => Set<ServicePrice>();

        // Orders
        public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
        public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<OrderItemTag> OrderItemTags => Set<OrderItemTag>();
        public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
        public DbSet<OrderCancellation> OrderCancellations => Set<OrderCancellation>();

        // Inspections
        public DbSet<InspectionIssueType> InspectionIssueTypes => Set<InspectionIssueType>();
        public DbSet<InspectionRecord> InspectionRecords => Set<InspectionRecord>();
        public DbSet<InspectionIssue> InspectionIssues => Set<InspectionIssue>();
        public DbSet<InspectionPhoto> InspectionPhotos => Set<InspectionPhoto>();

        // Transfers
        public DbSet<TransferType> TransferTypes => Set<TransferType>();
        public DbSet<TransferStatus> TransferStatuses => Set<TransferStatus>();
        public DbSet<AcknowledgementType> AcknowledgementTypes => Set<AcknowledgementType>();
        public DbSet<TransferBatch> TransferBatches => Set<TransferBatch>();
        public DbSet<TransferBatchItem> TransferBatchItems => Set<TransferBatchItem>();
        public DbSet<TransferAcknowledgement> TransferAcknowledgements => Set<TransferAcknowledgement>();

        // Plant Processing
        public DbSet<ProcessingStage> ProcessingStages => Set<ProcessingStage>();
        public DbSet<ProcessingStageStatus> ProcessingStageStatuses => Set<ProcessingStageStatus>();
        public DbSet<QCStatus> QCStatuses => Set<QCStatus>();
        public DbSet<PlantProcessing> PlantProcessings => Set<PlantProcessing>();
        public DbSet<PlantProcessingStageUpdate> PlantProcessingStageUpdates => Set<PlantProcessingStageUpdate>();
        public DbSet<QCRecord> QCRecords => Set<QCRecord>();

        // Collections
        public DbSet<CollectionMode> CollectionModes => Set<CollectionMode>();
        public DbSet<OrderCollection> OrderCollections => Set<OrderCollection>();

        // Payments
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<RefundStatus> RefundStatuses => Set<RefundStatus>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Refund> Refunds => Set<Refund>();

        // Notifications
        public DbSet<NotificationType> NotificationTypes => Set<NotificationType>();
        public DbSet<NotificationStatus> NotificationStatuses => Set<NotificationStatus>();
        public DbSet<Notification> Notifications => Set<Notification>();

        // Auditing
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityTables(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DobiDbContext).Assembly);

            LookupDataSeeder.Seed(builder);
        }

        private static void ConfigureIdentityTables(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(x => x.Id)
                    .HasColumnName("UserId");

                entity.Property(x => x.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.UserName)
                    .HasMaxLength(100);

                entity.Property(x => x.NormalizedUserName)
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .HasMaxLength(150);

                entity.Property(x => x.NormalizedEmail)
                    .HasMaxLength(150);

                entity.Property(x => x.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.HasOne<Branch>()
                    .WithMany()
                    .HasForeignKey(x => x.DefaultBranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Plant>()
                    .WithMany()
                    .HasForeignKey(x => x.DefaultPlantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("Roles");

                entity.Property(x => x.Id)
                    .HasColumnName("RoleId");

                entity.Property(x => x.Name)
                    .HasMaxLength(100);

                entity.Property(x => x.NormalizedName)
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(300);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });

            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
