using Dobi.Domain.Collections;
using Dobi.Domain.Customers;
using Dobi.Domain.Inspections;
using Dobi.Domain.Notifications;
using Dobi.Domain.Orders;
using Dobi.Domain.Payments;
using Dobi.Domain.PlantProcessing;
using Dobi.Domain.Services;
using Dobi.Domain.Transfers;
using Dobi.Infrastructure.Identity;
using Dobi.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Seed
{
    public static class LookupDataSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            SeedIdentityRoles(builder);
            SeedCustomerTypes(builder);
            SeedPricingTypes(builder);
            SeedInspectionIssueTypes(builder);
            SeedOrderStatuses(builder);
            SeedPaymentStatuses(builder);
            SeedTransferTypes(builder);
            SeedTransferStatuses(builder);
            SeedAcknowledgementTypes(builder);
            SeedProcessingStages(builder);
            SeedProcessingStageStatuses(builder);
            SeedQCStatuses(builder);
            SeedCollectionModes(builder);
            SeedPaymentMethods(builder);
            SeedRefundStatuses(builder);
            SeedNotificationTypes(builder);
            SeedNotificationStatuses(builder);
            SeedServices(builder);
            SeedItemCategories(builder);
        }

        private static void SeedIdentityRoles(ModelBuilder builder)
        {
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = 1,
                    Name = "ADMIN",
                    NormalizedName = "ADMIN",
                    Description = "Full system administration access.",
                    IsActive = true,
                    ConcurrencyStamp = "dobi-role-admin"
                },
                new ApplicationRole
                {
                    Id = 2,
                    Name = "OUTLET_STAFF",
                    NormalizedName = "OUTLET_STAFF",
                    Description = "Outlet order intake, customer handling, collection, and payment access.",
                    IsActive = true,
                    ConcurrencyStamp = "dobi-role-outlet-staff"
                },
                new ApplicationRole
                {
                    Id = 3,
                    Name = "PLANT_SUPERVISOR",
                    NormalizedName = "PLANT_SUPERVISOR",
                    Description = "Plant receipt, processing, QC, packing, and return workflow access.",
                    IsActive = true,
                    ConcurrencyStamp = "dobi-role-plant-supervisor"
                },
                new ApplicationRole
                {
                    Id = 4,
                    Name = "DRIVER",
                    NormalizedName = "DRIVER",
                    Description = "Driver pickup, drop-off, and acknowledgement access.",
                    IsActive = true,
                    ConcurrencyStamp = "dobi-role-driver"
                },
                new ApplicationRole
                {
                    Id = 5,
                    Name = "MANAGER",
                    NormalizedName = "MANAGER",
                    Description = "Operational monitoring and reporting access.",
                    IsActive = true,
                    ConcurrencyStamp = "dobi-role-manager"
                },
                new ApplicationRole
                {
                    Id = 6,
                    Name = "OPERATIONS_DIRECTOR",
                    NormalizedName = "OPERATIONS_DIRECTOR",
                    Description = "Overall operational visibility, reports, and audit review access.",
                    IsActive = true,
                    ConcurrencyStamp = "dobi-role-operations-director"
                }
            );
        }

        private static void SeedCustomerTypes(ModelBuilder builder)
        {
            builder.Entity<CustomerType>().HasData(
                new CustomerType
                {
                    Id = 1,
                    CustomerTypeCode = "B2C",
                    CustomerTypeName = "Individual Customer"
                },
                new CustomerType
                {
                    Id = 2,
                    CustomerTypeCode = "BULK",
                    CustomerTypeName = "Bulk / Corporate Customer"
                }
            );
        }

        private static void SeedPricingTypes(ModelBuilder builder)
        {
            builder.Entity<PricingType>().HasData(
                new PricingType
                {
                    Id = 1,
                    PricingTypeCode = "PER_KG",
                    PricingTypeName = "Per Kilogram"
                },
                new PricingType
                {
                    Id = 2,
                    PricingTypeCode = "PER_ITEM",
                    PricingTypeName = "Per Item"
                }
            );
        }

        private static void SeedInspectionIssueTypes(ModelBuilder builder)
        {
            builder.Entity<InspectionIssueType>().HasData(
                new InspectionIssueType
                {
                    Id = 1,
                    IssueCode = "STAIN",
                    IssueName = "Stain",
                    IsActive = true
                },
                new InspectionIssueType
                {
                    Id = 2,
                    IssueCode = "TEAR",
                    IssueName = "Tear",
                    IsActive = true
                },
                new InspectionIssueType
                {
                    Id = 3,
                    IssueCode = "MISSING_BUTTONS",
                    IssueName = "Missing Buttons",
                    IsActive = true
                },
                new InspectionIssueType
                {
                    Id = 4,
                    IssueCode = "COLOR_FADING",
                    IssueName = "Colour Fading",
                    IsActive = true
                },
                new InspectionIssueType
                {
                    Id = 5,
                    IssueCode = "FABRIC_DAMAGE",
                    IssueName = "Fabric Damage",
                    IsActive = true
                },
                new InspectionIssueType
                {
                    Id = 6,
                    IssueCode = "OTHER",
                    IssueName = "Other",
                    IsActive = true
                }
            );
        }

        private static void SeedOrderStatuses(ModelBuilder builder)
        {
            builder.Entity<OrderStatus>().HasData(
                new OrderStatus
                {
                    Id = 1,
                    StatusCode = "DRAFT",
                    StatusName = "Draft",
                    SortOrder = 1,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 2,
                    StatusCode = "CREATED",
                    StatusName = "Created",
                    SortOrder = 2,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 3,
                    StatusCode = "CANCELLED",
                    StatusName = "Cancelled",
                    SortOrder = 3,
                    IsTerminal = true
                },
                new OrderStatus
                {
                    Id = 4,
                    StatusCode = "SENT_TO_PLANT",
                    StatusName = "Sent to Plant",
                    SortOrder = 4,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 5,
                    StatusCode = "RECEIVED_AT_PLANT",
                    StatusName = "Received at Plant",
                    SortOrder = 5,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 6,
                    StatusCode = "PROCESSING",
                    StatusName = "Processing",
                    SortOrder = 6,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 7,
                    StatusCode = "QC_PENDING",
                    StatusName = "QC Pending",
                    SortOrder = 7,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 8,
                    StatusCode = "QC_FAILED",
                    StatusName = "QC Failed",
                    SortOrder = 8,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 9,
                    StatusCode = "PACKED",
                    StatusName = "Packed",
                    SortOrder = 9,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 10,
                    StatusCode = "READY_FOR_OUTLET_RETURN",
                    StatusName = "Ready for Outlet Return",
                    SortOrder = 10,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 11,
                    StatusCode = "RETURNED_TO_OUTLET",
                    StatusName = "Returned to Outlet",
                    SortOrder = 11,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 12,
                    StatusCode = "READY_FOR_COLLECTION",
                    StatusName = "Ready for Collection",
                    SortOrder = 12,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 13,
                    StatusCode = "COLLECTED_DELIVERED",
                    StatusName = "Collected/Delivered",
                    SortOrder = 13,
                    IsTerminal = false
                },
                new OrderStatus
                {
                    Id = 14,
                    StatusCode = "CLOSED",
                    StatusName = "Closed",
                    SortOrder = 14,
                    IsTerminal = true
                }
            );
        }

        private static void SeedPaymentStatuses(ModelBuilder builder)
        {
            builder.Entity<PaymentStatus>().HasData(
                new PaymentStatus
                {
                    Id = 1,
                    StatusCode = PaymentStatusCodes.Unpaid,
                    StatusName = "Unpaid"
                },
                new PaymentStatus
                {
                    Id = 2,
                    StatusCode = PaymentStatusCodes.Paid,
                    StatusName = "Paid"
                },
                new PaymentStatus
                {
                    Id = 3,
                    StatusCode = PaymentStatusCodes.Refunded,
                    StatusName = "Refunded"
                },
                new PaymentStatus
                {
                    Id = 4,
                    StatusCode = PaymentStatusCodes.Failed,
                    StatusName = "Failed"
                },
                new PaymentStatus
                {
                    Id = 5,
                    StatusCode = PaymentStatusCodes.PendingClearance,
                    StatusName = "Pending Clearance"
                },
                new PaymentStatus
                {
                    Id = 6,
                    StatusCode = PaymentStatusCodes.PartiallyPaid,
                    StatusName = "Partially Paid"
                },
                new PaymentStatus
                {
                    Id = 7,
                    StatusCode = PaymentStatusCodes.PartiallyPaidPendingClearance,
                    StatusName = "Partially Paid - Pending Clearance"
                }
            );
        }

        private static void SeedTransferTypes(ModelBuilder builder)
        {
            builder.Entity<TransferType>().HasData(
                new TransferType
                {
                    Id = 1,
                    TransferTypeCode = "OUTLET_TO_PLANT",
                    TransferTypeName = "Outlet to Plant"
                },
                new TransferType
                {
                    Id = 2,
                    TransferTypeCode = "PLANT_TO_OUTLET",
                    TransferTypeName = "Plant to Outlet"
                }
            );
        }

        private static void SeedTransferStatuses(ModelBuilder builder)
        {
            builder.Entity<TransferStatus>().HasData(
                new TransferStatus
                {
                    Id = 1,
                    StatusCode = "CREATED",
                    StatusName = "Created"
                },
                new TransferStatus
                {
                    Id = 2,
                    StatusCode = "IN_TRANSIT",
                    StatusName = "In Transit"
                },
                new TransferStatus
                {
                    Id = 3,
                    StatusCode = "RECEIVED",
                    StatusName = "Received"
                }
            );
        }

        private static void SeedAcknowledgementTypes(ModelBuilder builder)
        {
            builder.Entity<AcknowledgementType>().HasData(
                new AcknowledgementType
                {
                    Id = 1,
                    AcknowledgementCode = "OUTLET_HANDOVER",
                    AcknowledgementName = "Outlet Handover"
                },
                new AcknowledgementType
                {
                    Id = 2,
                    AcknowledgementCode = "DRIVER_PICKUP",
                    AcknowledgementName = "Driver Pickup"
                },
                new AcknowledgementType
                {
                    Id = 3,
                    AcknowledgementCode = "PLANT_RECEIVE",
                    AcknowledgementName = "Plant Receive"
                },
                new AcknowledgementType
                {
                    Id = 4,
                    AcknowledgementCode = "PLANT_HANDOVER",
                    AcknowledgementName = "Plant Handover"
                },
                new AcknowledgementType
                {
                    Id = 5,
                    AcknowledgementCode = "DRIVER_DROPOFF",
                    AcknowledgementName = "Driver Drop-off"
                },
                new AcknowledgementType
                {
                    Id = 6,
                    AcknowledgementCode = "OUTLET_RECEIVE",
                    AcknowledgementName = "Outlet Receive"
                }
            );
        }

        private static void SeedProcessingStages(ModelBuilder builder)
        {
            builder.Entity<ProcessingStage>().HasData(
                new ProcessingStage
                {
                    Id = 1,
                    StageCode = "WASHING",
                    StageName = "Washing",
                    SortOrder = 1,
                    IsActive = true
                },
                new ProcessingStage
                {
                    Id = 2,
                    StageCode = "DRYING",
                    StageName = "Drying",
                    SortOrder = 2,
                    IsActive = true
                },
                new ProcessingStage
                {
                    Id = 3,
                    StageCode = "IRONING",
                    StageName = "Ironing",
                    SortOrder = 3,
                    IsActive = true
                },
                new ProcessingStage
                {
                    Id = 4,
                    StageCode = "QC",
                    StageName = "Quality Check",
                    SortOrder = 4,
                    IsActive = true
                },
                new ProcessingStage
                {
                    Id = 5,
                    StageCode = "PACKING",
                    StageName = "Packing",
                    SortOrder = 5,
                    IsActive = true
                }
            );
        }

        private static void SeedProcessingStageStatuses(ModelBuilder builder)
        {
            builder.Entity<ProcessingStageStatus>().HasData(
                new ProcessingStageStatus
                {
                    Id = 1,
                    StatusCode = "PENDING",
                    StatusName = "Pending"
                },
                new ProcessingStageStatus
                {
                    Id = 2,
                    StatusCode = "IN_PROGRESS",
                    StatusName = "In Progress"
                },
                new ProcessingStageStatus
                {
                    Id = 3,
                    StatusCode = "DONE",
                    StatusName = "Done"
                },
                new ProcessingStageStatus
                {
                    Id = 4,
                    StatusCode = "FAILED",
                    StatusName = "Failed"
                },
                new ProcessingStageStatus
                {
                    Id = 5,
                    StatusCode = "NOT_REQUIRED",
                    StatusName = "Not Required"
                }
            );
        }

        private static void SeedQCStatuses(ModelBuilder builder)
        {
            builder.Entity<QCStatus>().HasData(
                new QCStatus
                {
                    Id = 1,
                    QCStatusCode = "PASSED",
                    QCStatusName = "Passed"
                },
                new QCStatus
                {
                    Id = 2,
                    QCStatusCode = "FAILED",
                    QCStatusName = "Failed"
                }
            );
        }

        private static void SeedCollectionModes(ModelBuilder builder)
        {
            builder.Entity<CollectionMode>().HasData(
                new CollectionMode
                {
                    Id = 1,
                    CollectionModeCode = "COLLECTION",
                    CollectionModeName = "Customer Collection"
                },
                new CollectionMode
                {
                    Id = 2,
                    CollectionModeCode = "DELIVERY",
                    CollectionModeName = "Delivery"
                }
            );
        }

        private static void SeedPaymentMethods(ModelBuilder builder)
        {
            builder.Entity<PaymentMethod>().HasData(
                new PaymentMethod
                {
                    Id = 1,
                    MethodCode = PaymentMethodCodes.Cash,
                    MethodName = "Cash"
                },
                new PaymentMethod
                {
                    Id = 2,
                    MethodCode = PaymentMethodCodes.Card,
                    MethodName = "Card"
                },
                new PaymentMethod
                {
                    Id = 3,
                    MethodCode = PaymentMethodCodes.BankTransfer,
                    MethodName = "Bank Transfer"
                },
                new PaymentMethod
                {
                    Id = 4,
                    MethodCode = PaymentMethodCodes.Cheque,
                    MethodName = "Cheque"
                }
            );
        }

        private static void SeedRefundStatuses(ModelBuilder builder)
        {
            builder.Entity<RefundStatus>().HasData(
                new RefundStatus
                {
                    Id = 1,
                    StatusCode = "PENDING",
                    StatusName = "Pending"
                },
                new RefundStatus
                {
                    Id = 2,
                    StatusCode = "APPROVED",
                    StatusName = "Approved"
                },
                new RefundStatus
                {
                    Id = 3,
                    StatusCode = "PAID",
                    StatusName = "Paid"
                },
                new RefundStatus
                {
                    Id = 4,
                    StatusCode = "REJECTED",
                    StatusName = "Rejected"
                }
            );
        }

        private static void SeedNotificationTypes(ModelBuilder builder)
        {
            builder.Entity<NotificationType>().HasData(
                new NotificationType
                {
                    Id = 1,
                    TypeCode = "SMS",
                    TypeName = "SMS"
                }
            );
        }

        private static void SeedNotificationStatuses(ModelBuilder builder)
        {
            builder.Entity<NotificationStatus>().HasData(
                new NotificationStatus
                {
                    Id = 1,
                    StatusCode = "PENDING",
                    StatusName = "Pending"
                },
                new NotificationStatus
                {
                    Id = 2,
                    StatusCode = "SENT",
                    StatusName = "Sent"
                },
                new NotificationStatus
                {
                    Id = 3,
                    StatusCode = "FAILED",
                    StatusName = "Failed"
                }
            );
        }

        private static void SeedServices(ModelBuilder builder)
        {
            var createdAt = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    ServiceCode = "WASH_FOLD",
                    ServiceName = "Wash & Fold",
                    Description = "Standard washing and folding service.",
                    IsExpressEligible = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new Service
                {
                    Id = 2,
                    ServiceCode = "WASH_IRON",
                    ServiceName = "Wash & Iron",
                    Description = "Washing and ironing service.",
                    IsExpressEligible = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new Service
                {
                    Id = 3,
                    ServiceCode = "IRONING_ONLY",
                    ServiceName = "Ironing Only",
                    Description = "Ironing service only.",
                    IsExpressEligible = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new Service
                {
                    Id = 4,
                    ServiceCode = "PRESSING",
                    ServiceName = "Pressing",
                    Description = "Pressing service.",
                    IsExpressEligible = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new Service
                {
                    Id = 5,
                    ServiceCode = "DRY_CLEANING",
                    ServiceName = "Dry Cleaning",
                    Description = "Dry cleaning coordination service.",
                    IsExpressEligible = false,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new Service
                {
                    Id = 6,
                    ServiceCode = "DENIM_ACID_WASHING",
                    ServiceName = "Denim Acid Washing",
                    Description = "Special denim acid washing service.",
                    IsExpressEligible = false,
                    IsActive = true,
                    CreatedAt = createdAt
                }
            );
        }

        private static void SeedItemCategories(ModelBuilder builder)
        {
            var createdAt = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<ItemCategory>().HasData(
                new ItemCategory
                {
                    Id = 1,
                    CategoryName = "Shirt",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = false,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 2,
                    CategoryName = "Trouser",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = false,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 3,
                    CategoryName = "Jacket",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 4,
                    CategoryName = "Kurtha",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 5,
                    CategoryName = "Saree",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 6,
                    CategoryName = "Bedsheet",
                    DefaultPricingTypeId = 1,
                    IsSpecialItem = false,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 7,
                    CategoryName = "Curtain",
                    DefaultPricingTypeId = 1,
                    IsSpecialItem = true,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 8,
                    CategoryName = "Linen",
                    DefaultPricingTypeId = 1,
                    IsSpecialItem = false,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 9,
                    CategoryName = "Uniform",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = false,
                    IsActive = true,
                    CreatedAt = createdAt
                },
                new ItemCategory
                {
                    Id = 10,
                    CategoryName = "Other",
                    DefaultPricingTypeId = 2,
                    IsSpecialItem = false,
                    IsActive = true,
                    CreatedAt = createdAt
                }
            );
        }
    }
}
