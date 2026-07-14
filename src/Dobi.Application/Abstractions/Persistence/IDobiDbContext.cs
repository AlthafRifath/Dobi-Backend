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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Persistence
{
    public interface IDobiDbContext
    {
        // Branches / Plants
        DbSet<Branch> Branches { get; }
        DbSet<Plant> Plants { get; }

        // Dobi-specific user assignments
        DbSet<UserBranchAssignment> UserBranchAssignments { get; }
        DbSet<UserPlantAssignment> UserPlantAssignments { get; }

        // Customers
        DbSet<CustomerType> CustomerTypes { get; }
        DbSet<Customer> Customers { get; }

        // Services / Pricing
        DbSet<PricingType> PricingTypes { get; }
        DbSet<Dobi.Domain.Services.Service> Services { get; }
        DbSet<ItemCategory> ItemCategories { get; }
        DbSet<ServicePrice> ServicePrices { get; }

        // Orders
        DbSet<OrderStatus> OrderStatuses { get; }
        DbSet<PaymentStatus> PaymentStatuses { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<OrderItemTag> OrderItemTags { get; }
        DbSet<OrderStatusHistory> OrderStatusHistories { get; }
        DbSet<OrderCancellation> OrderCancellations { get; }

        // Inspections
        DbSet<InspectionIssueType> InspectionIssueTypes { get; }
        DbSet<InspectionRecord> InspectionRecords { get; }
        DbSet<InspectionIssue> InspectionIssues { get; }
        DbSet<InspectionPhoto> InspectionPhotos { get; }

        // Transfers
        DbSet<TransferType> TransferTypes { get; }
        DbSet<TransferStatus> TransferStatuses { get; }
        DbSet<AcknowledgementType> AcknowledgementTypes { get; }
        DbSet<TransferBatch> TransferBatches { get; }
        DbSet<TransferBatchItem> TransferBatchItems { get; }
        DbSet<TransferAcknowledgement> TransferAcknowledgements { get; }

        // Plant Processing
        DbSet<ProcessingStage> ProcessingStages { get; }
        DbSet<ProcessingStageStatus> ProcessingStageStatuses { get; }
        DbSet<QCStatus> QCStatuses { get; }
        DbSet<PlantProcessing> PlantProcessings { get; }
        DbSet<PlantProcessingStageUpdate> PlantProcessingStageUpdates { get; }
        DbSet<QCRecord> QCRecords { get; }

        // Collections
        DbSet<CollectionMode> CollectionModes { get; }
        DbSet<OrderCollection> OrderCollections { get; }

        // Payments
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<RefundStatus> RefundStatuses { get; }
        DbSet<Payment> Payments { get; }
        DbSet<Refund> Refunds { get; }

        // Notifications
        DbSet<NotificationType> NotificationTypes { get; }
        DbSet<NotificationStatus> NotificationStatuses { get; }
        DbSet<Notification> Notifications { get; }

        // Auditing
        DbSet<AuditLog> AuditLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
