using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dobi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcknowledgementTypes",
                columns: table => new
                {
                    AcknowledgementTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcknowledgementCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AcknowledgementName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcknowledgementTypes", x => x.AcknowledgementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ContactNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "CollectionModes",
                columns: table => new
                {
                    CollectionModeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CollectionModeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CollectionModeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionModes", x => x.CollectionModeId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CustomerTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "InspectionIssueTypes",
                columns: table => new
                {
                    InspectionIssueTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IssueCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IssueName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionIssueTypes", x => x.InspectionIssueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationStatuses",
                columns: table => new
                {
                    NotificationStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatuses", x => x.NotificationStatusId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.NotificationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.OrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MethodCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MethodName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.PaymentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    OperatingHours = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.PlantId);
                });

            migrationBuilder.CreateTable(
                name: "PricingTypes",
                columns: table => new
                {
                    PricingTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PricingTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PricingTypeName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingTypes", x => x.PricingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingStages",
                columns: table => new
                {
                    ProcessingStageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StageCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StageName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStages", x => x.ProcessingStageId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingStageStatuses",
                columns: table => new
                {
                    ProcessingStageStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStageStatuses", x => x.ProcessingStageStatusId);
                });

            migrationBuilder.CreateTable(
                name: "QCStatuses",
                columns: table => new
                {
                    QCStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QCStatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QCStatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCStatuses", x => x.QCStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RefundStatuses",
                columns: table => new
                {
                    RefundStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundStatuses", x => x.RefundStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IsExpressEligible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "TransferStatuses",
                columns: table => new
                {
                    TransferStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStatuses", x => x.TransferStatusId);
                });

            migrationBuilder.CreateTable(
                name: "TransferTypes",
                columns: table => new
                {
                    TransferTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransferTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferTypes", x => x.TransferTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CustomerTypeId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MobileNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerTypes_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "CustomerTypes",
                        principalColumn: "CustomerTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DefaultBranchId = table.Column<int>(type: "integer", nullable: true),
                    DefaultPlantId = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Branches_DefaultBranchId",
                        column: x => x.DefaultBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Plants_DefaultPlantId",
                        column: x => x.DefaultPlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ItemCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DefaultPricingTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsSpecialItem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.ItemCategoryId);
                    table.ForeignKey(
                        name: "FK_ItemCategories_PricingTypes_DefaultPricingTypeId",
                        column: x => x.DefaultPricingTypeId,
                        principalTable: "PricingTypes",
                        principalColumn: "PricingTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OldValue = table.Column<string>(type: "jsonb", nullable: true),
                    NewValue = table.Column<string>(type: "jsonb", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogId);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedReturnDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CurrentStatusId = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false),
                    IsExpress = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SubTotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    ExpressChargeAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_PaymentStatuses_PaymentStatusId",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatuses",
                        principalColumn: "PaymentStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferBatches",
                columns: table => new
                {
                    TransferBatchId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TransferTypeId = table.Column<int>(type: "integer", nullable: false),
                    FromBranchId = table.Column<int>(type: "integer", nullable: true),
                    FromPlantId = table.Column<int>(type: "integer", nullable: true),
                    ToBranchId = table.Column<int>(type: "integer", nullable: true),
                    ToPlantId = table.Column<int>(type: "integer", nullable: true),
                    DriverUserId = table.Column<int>(type: "integer", nullable: false),
                    TransferStatusId = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferBatches", x => x.TransferBatchId);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Branches_FromBranchId",
                        column: x => x.FromBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Branches_ToBranchId",
                        column: x => x.ToBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Plants_FromPlantId",
                        column: x => x.FromPlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Plants_ToPlantId",
                        column: x => x.ToPlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_TransferStatuses_TransferStatusId",
                        column: x => x.TransferStatusId,
                        principalTable: "TransferStatuses",
                        principalColumn: "TransferStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_TransferTypes_TransferTypeId",
                        column: x => x.TransferTypeId,
                        principalTable: "TransferTypes",
                        principalColumn: "TransferTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Users_DriverUserId",
                        column: x => x.DriverUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatches_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBranchAssignments",
                columns: table => new
                {
                    UserBranchAssignmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBranchAssignments", x => x.UserBranchAssignmentId);
                    table.ForeignKey(
                        name: "FK_UserBranchAssignments_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBranchAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlantAssignments",
                columns: table => new
                {
                    UserPlantAssignmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PlantId = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlantAssignments", x => x.UserPlantAssignmentId);
                    table.ForeignKey(
                        name: "FK_UserPlantAssignments_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPlantAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePrices",
                columns: table => new
                {
                    ServicePriceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    ItemCategoryId = table.Column<int>(type: "integer", nullable: true),
                    PricingTypeId = table.Column<int>(type: "integer", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpressAdditionalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EffectiveFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveTo = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePrices", x => x.ServicePriceId);
                    table.ForeignKey(
                        name: "FK_ServicePrices_ItemCategories_ItemCategoryId",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePrices_PricingTypes_PricingTypeId",
                        column: x => x.PricingTypeId,
                        principalTable: "PricingTypes",
                        principalColumn: "PricingTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePrices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: true),
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false),
                    Recipient = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    NotificationStatusId = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationStatuses_NotificationStatusId",
                        column: x => x.NotificationStatusId,
                        principalTable: "NotificationStatuses",
                        principalColumn: "NotificationStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "NotificationTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderCancellations",
                columns: table => new
                {
                    OrderCancellationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    CancellationReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RequestedByCustomer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CancelledByUserId = table.Column<int>(type: "integer", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCancellations", x => x.OrderCancellationId);
                    table.ForeignKey(
                        name: "FK_OrderCancellations_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderCancellations_Users_CancelledByUserId",
                        column: x => x.CancelledByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderCollections",
                columns: table => new
                {
                    OrderCollectionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    CollectionModeId = table.Column<int>(type: "integer", nullable: false),
                    IsCollectedByCustomer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CollectorName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CollectorMobileNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ReceiptVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MobileNoVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ReceiptImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CustomerSignatureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CollectedOrDeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReleasedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCollections", x => x.OrderCollectionId);
                    table.ForeignKey(
                        name: "FK_OrderCollections_CollectionModes_CollectionModeId",
                        column: x => x.CollectionModeId,
                        principalTable: "CollectionModes",
                        principalColumn: "CollectionModeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderCollections_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderCollections_Users_ReleasedByUserId",
                        column: x => x.ReleasedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistories",
                columns: table => new
                {
                    StatusHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    OrderStatusId = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChangedByUserId = table.Column<int>(type: "integer", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistories", x => x.StatusHistoryId);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_Users_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReceivedByUserId = table.Column<int>(type: "integer", nullable: true),
                    ReferenceNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentStatuses_PaymentStatusId",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatuses",
                        principalColumn: "PaymentStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Users_ReceivedByUserId",
                        column: x => x.ReceivedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlantProcessings",
                columns: table => new
                {
                    PlantProcessingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    PlantId = table.Column<int>(type: "integer", nullable: false),
                    ReceivedAtPlant = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReadyDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OverallQCStatusId = table.Column<int>(type: "integer", nullable: true),
                    PlantRemarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantProcessings", x => x.PlantProcessingId);
                    table.ForeignKey(
                        name: "FK_PlantProcessings_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantProcessings_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantProcessings_QCStatuses_OverallQCStatusId",
                        column: x => x.OverallQCStatusId,
                        principalTable: "QCStatuses",
                        principalColumn: "QCStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantProcessings_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantProcessings_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferAcknowledgements",
                columns: table => new
                {
                    TransferAcknowledgementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferBatchId = table.Column<int>(type: "integer", nullable: false),
                    AcknowledgementTypeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SignatureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferAcknowledgements", x => x.TransferAcknowledgementId);
                    table.ForeignKey(
                        name: "FK_TransferAcknowledgements_AcknowledgementTypes_Acknowledgeme~",
                        column: x => x.AcknowledgementTypeId,
                        principalTable: "AcknowledgementTypes",
                        principalColumn: "AcknowledgementTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferAcknowledgements_TransferBatches_TransferBatchId",
                        column: x => x.TransferBatchId,
                        principalTable: "TransferBatches",
                        principalColumn: "TransferBatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferAcknowledgements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferBatchItems",
                columns: table => new
                {
                    TransferBatchItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferBatchId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    NoOfBags = table.Column<int>(type: "integer", nullable: false),
                    NoOfPieces = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferBatchItems", x => x.TransferBatchItemId);
                    table.ForeignKey(
                        name: "FK_TransferBatchItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferBatchItems_TransferBatches_TransferBatchId",
                        column: x => x.TransferBatchId,
                        principalTable: "TransferBatches",
                        principalColumn: "TransferBatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    ItemCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ServicePriceId = table.Column<int>(type: "integer", nullable: true),
                    PricingTypeId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    LineAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SpecialNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_ItemCategories_ItemCategoryId",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_PricingTypes_PricingTypeId",
                        column: x => x.PricingTypeId,
                        principalTable: "PricingTypes",
                        principalColumn: "PricingTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_ServicePrices_ServicePriceId",
                        column: x => x.ServicePriceId,
                        principalTable: "ServicePrices",
                        principalColumn: "ServicePriceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    RefundId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    PaymentId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RefundStatusId = table.Column<int>(type: "integer", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "integer", nullable: true),
                    RefundedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_Refunds_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_RefundStatuses_RefundStatusId",
                        column: x => x.RefundStatusId,
                        principalTable: "RefundStatuses",
                        principalColumn: "RefundStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Users_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlantProcessingStageUpdates",
                columns: table => new
                {
                    PlantProcessingStageUpdateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantProcessingId = table.Column<int>(type: "integer", nullable: false),
                    ProcessingStageId = table.Column<int>(type: "integer", nullable: false),
                    ProcessingStageStatusId = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantProcessingStageUpdates", x => x.PlantProcessingStageUpdateId);
                    table.ForeignKey(
                        name: "FK_PlantProcessingStageUpdates_PlantProcessings_PlantProcessin~",
                        column: x => x.PlantProcessingId,
                        principalTable: "PlantProcessings",
                        principalColumn: "PlantProcessingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantProcessingStageUpdates_ProcessingStageStatuses_Process~",
                        column: x => x.ProcessingStageStatusId,
                        principalTable: "ProcessingStageStatuses",
                        principalColumn: "ProcessingStageStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantProcessingStageUpdates_ProcessingStages_ProcessingStag~",
                        column: x => x.ProcessingStageId,
                        principalTable: "ProcessingStages",
                        principalColumn: "ProcessingStageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantProcessingStageUpdates_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionRecords",
                columns: table => new
                {
                    InspectionRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    OrderItemId = table.Column<int>(type: "integer", nullable: true),
                    CustomerAcknowledged = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CustomerSignatureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    InspectedByUserId = table.Column<int>(type: "integer", nullable: false),
                    InspectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionRecords", x => x.InspectionRecordId);
                    table.ForeignKey(
                        name: "FK_InspectionRecords_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionRecords_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionRecords_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionRecords_Users_InspectedByUserId",
                        column: x => x.InspectedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionRecords_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemTags",
                columns: table => new
                {
                    OrderItemTagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderItemId = table.Column<int>(type: "integer", nullable: false),
                    TagNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PieceNo = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemTags", x => x.OrderItemTagId);
                    table.ForeignKey(
                        name: "FK_OrderItemTags_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemTags_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemTags_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QCRecords",
                columns: table => new
                {
                    QCRecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantProcessingId = table.Column<int>(type: "integer", nullable: false),
                    OrderItemId = table.Column<int>(type: "integer", nullable: true),
                    QCStatusId = table.Column<int>(type: "integer", nullable: false),
                    IssueDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ActionTaken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LabourChargeAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RecordedByUserId = table.Column<int>(type: "integer", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCRecords", x => x.QCRecordId);
                    table.ForeignKey(
                        name: "FK_QCRecords_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QCRecords_PlantProcessings_PlantProcessingId",
                        column: x => x.PlantProcessingId,
                        principalTable: "PlantProcessings",
                        principalColumn: "PlantProcessingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QCRecords_QCStatuses_QCStatusId",
                        column: x => x.QCStatusId,
                        principalTable: "QCStatuses",
                        principalColumn: "QCStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QCRecords_Users_RecordedByUserId",
                        column: x => x.RecordedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionIssues",
                columns: table => new
                {
                    InspectionIssueId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InspectionRecordId = table.Column<int>(type: "integer", nullable: false),
                    InspectionIssueTypeId = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionIssues", x => x.InspectionIssueId);
                    table.ForeignKey(
                        name: "FK_InspectionIssues_InspectionIssueTypes_InspectionIssueTypeId",
                        column: x => x.InspectionIssueTypeId,
                        principalTable: "InspectionIssueTypes",
                        principalColumn: "InspectionIssueTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionIssues_InspectionRecords_InspectionRecordId",
                        column: x => x.InspectionRecordId,
                        principalTable: "InspectionRecords",
                        principalColumn: "InspectionRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionPhotos",
                columns: table => new
                {
                    InspectionPhotoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InspectionRecordId = table.Column<int>(type: "integer", nullable: false),
                    PhotoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UploadedByUserId = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionPhotos", x => x.InspectionPhotoId);
                    table.ForeignKey(
                        name: "FK_InspectionPhotos_InspectionRecords_InspectionRecordId",
                        column: x => x.InspectionRecordId,
                        principalTable: "InspectionRecords",
                        principalColumn: "InspectionRecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectionPhotos_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionPhotos_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionPhotos_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AcknowledgementTypes",
                columns: new[] { "AcknowledgementTypeId", "AcknowledgementCode", "AcknowledgementName" },
                values: new object[,]
                {
                    { 1, "OUTLET_HANDOVER", "Outlet Handover" },
                    { 2, "DRIVER_PICKUP", "Driver Pickup" },
                    { 3, "PLANT_RECEIVE", "Plant Receive" },
                    { 4, "PLANT_HANDOVER", "Plant Handover" },
                    { 5, "DRIVER_DROPOFF", "Driver Drop-off" },
                    { 6, "OUTLET_RECEIVE", "Outlet Receive" }
                });

            migrationBuilder.InsertData(
                table: "CollectionModes",
                columns: new[] { "CollectionModeId", "CollectionModeCode", "CollectionModeName" },
                values: new object[,]
                {
                    { 1, "COLLECTION", "Customer Collection" },
                    { 2, "DELIVERY", "Delivery" }
                });

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "CustomerTypeId", "CustomerTypeCode", "CustomerTypeName" },
                values: new object[,]
                {
                    { 1, "B2C", "Individual Customer" },
                    { 2, "BULK", "Bulk / Corporate Customer" }
                });

            migrationBuilder.InsertData(
                table: "InspectionIssueTypes",
                columns: new[] { "InspectionIssueTypeId", "IsActive", "IssueCode", "IssueName" },
                values: new object[,]
                {
                    { 1, true, "STAIN", "Stain" },
                    { 2, true, "TEAR", "Tear" },
                    { 3, true, "MISSING_BUTTONS", "Missing Buttons" },
                    { 4, true, "COLOR_FADING", "Colour Fading" },
                    { 5, true, "FABRIC_DAMAGE", "Fabric Damage" },
                    { 6, true, "OTHER", "Other" }
                });

            migrationBuilder.InsertData(
                table: "NotificationStatuses",
                columns: new[] { "NotificationStatusId", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 1, "PENDING", "Pending" },
                    { 2, "SENT", "Sent" },
                    { 3, "FAILED", "Failed" }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "TypeCode", "TypeName" },
                values: new object[] { 1, "SMS", "SMS" });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "SortOrder", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 1, 1, "DRAFT", "Draft" },
                    { 2, 2, "CREATED", "Created" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "IsTerminal", "SortOrder", "StatusCode", "StatusName" },
                values: new object[] { 3, true, 3, "CANCELLED", "Cancelled" });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "SortOrder", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 4, 4, "SENT_TO_PLANT", "Sent to Plant" },
                    { 5, 5, "RECEIVED_AT_PLANT", "Received at Plant" },
                    { 6, 6, "PROCESSING", "Processing" },
                    { 7, 7, "QC_PENDING", "QC Pending" },
                    { 8, 8, "QC_FAILED", "QC Failed" },
                    { 9, 9, "PACKED", "Packed" },
                    { 10, 10, "READY_FOR_OUTLET_RETURN", "Ready for Outlet Return" },
                    { 11, 11, "RETURNED_TO_OUTLET", "Returned to Outlet" },
                    { 12, 12, "READY_FOR_COLLECTION", "Ready for Collection" },
                    { 13, 13, "COLLECTED_DELIVERED", "Collected/Delivered" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "IsTerminal", "SortOrder", "StatusCode", "StatusName" },
                values: new object[] { 14, true, 14, "CLOSED", "Closed" });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodId", "MethodCode", "MethodName" },
                values: new object[,]
                {
                    { 1, "CASH", "Cash" },
                    { 2, "CARD", "Card" },
                    { 3, "BANK_TRANSFER", "Bank Transfer" }
                });

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "PaymentStatusId", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 1, "UNPAID", "Unpaid" },
                    { 2, "PAID", "Paid" },
                    { 3, "REFUNDED", "Refunded" },
                    { 4, "FAILED", "Failed" }
                });

            migrationBuilder.InsertData(
                table: "PricingTypes",
                columns: new[] { "PricingTypeId", "PricingTypeCode", "PricingTypeName" },
                values: new object[,]
                {
                    { 1, "PER_KG", "Per Kilogram" },
                    { 2, "PER_ITEM", "Per Item" }
                });

            migrationBuilder.InsertData(
                table: "ProcessingStageStatuses",
                columns: new[] { "ProcessingStageStatusId", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 1, "PENDING", "Pending" },
                    { 2, "IN_PROGRESS", "In Progress" },
                    { 3, "DONE", "Done" },
                    { 4, "FAILED", "Failed" },
                    { 5, "NOT_REQUIRED", "Not Required" }
                });

            migrationBuilder.InsertData(
                table: "ProcessingStages",
                columns: new[] { "ProcessingStageId", "IsActive", "SortOrder", "StageCode", "StageName" },
                values: new object[,]
                {
                    { 1, true, 1, "WASHING", "Washing" },
                    { 2, true, 2, "DRYING", "Drying" },
                    { 3, true, 3, "IRONING", "Ironing" },
                    { 4, true, 4, "QC", "Quality Check" },
                    { 5, true, 5, "PACKING", "Packing" }
                });

            migrationBuilder.InsertData(
                table: "QCStatuses",
                columns: new[] { "QCStatusId", "QCStatusCode", "QCStatusName" },
                values: new object[,]
                {
                    { 1, "PASSED", "Passed" },
                    { 2, "FAILED", "Failed" }
                });

            migrationBuilder.InsertData(
                table: "RefundStatuses",
                columns: new[] { "RefundStatusId", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 1, "PENDING", "Pending" },
                    { 2, "APPROVED", "Approved" },
                    { 3, "PAID", "Paid" },
                    { 4, "REJECTED", "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "ConcurrencyStamp", "Description", "IsActive", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "dobi-role-admin", "Full system administration access.", true, "ADMIN", "ADMIN" },
                    { 2, "dobi-role-outlet-staff", "Outlet order intake, customer handling, collection, and payment access.", true, "OUTLET_STAFF", "OUTLET_STAFF" },
                    { 3, "dobi-role-plant-supervisor", "Plant receipt, processing, QC, packing, and return workflow access.", true, "PLANT_SUPERVISOR", "PLANT_SUPERVISOR" },
                    { 4, "dobi-role-driver", "Driver pickup, drop-off, and acknowledgement access.", true, "DRIVER", "DRIVER" },
                    { 5, "dobi-role-manager", "Operational monitoring and reporting access.", true, "MANAGER", "MANAGER" },
                    { 6, "dobi-role-operations-director", "Overall operational visibility, reports, and audit review access.", true, "OPERATIONS_DIRECTOR", "OPERATIONS_DIRECTOR" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "CreatedAt", "CreatedByUserId", "Description", "IsActive", "IsExpressEligible", "ServiceCode", "ServiceName", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Standard washing and folding service.", true, true, "WASH_FOLD", "Wash & Fold", null, null },
                    { 2, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Washing and ironing service.", true, true, "WASH_IRON", "Wash & Iron", null, null },
                    { 3, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ironing service only.", true, true, "IRONING_ONLY", "Ironing Only", null, null },
                    { 4, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pressing service.", true, true, "PRESSING", "Pressing", null, null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "CreatedAt", "CreatedByUserId", "Description", "IsActive", "ServiceCode", "ServiceName", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 5, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dry cleaning coordination service.", true, "DRY_CLEANING", "Dry Cleaning", null, null },
                    { 6, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Special denim acid washing service.", true, "DENIM_ACID_WASHING", "Denim Acid Washing", null, null }
                });

            migrationBuilder.InsertData(
                table: "TransferStatuses",
                columns: new[] { "TransferStatusId", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 1, "CREATED", "Created" },
                    { 2, "IN_TRANSIT", "In Transit" },
                    { 3, "RECEIVED", "Received" }
                });

            migrationBuilder.InsertData(
                table: "TransferTypes",
                columns: new[] { "TransferTypeId", "TransferTypeCode", "TransferTypeName" },
                values: new object[,]
                {
                    { 1, "OUTLET_TO_PLANT", "Outlet to Plant" },
                    { 2, "PLANT_TO_OUTLET", "Plant to Outlet" }
                });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryId", "CategoryName", "CreatedAt", "CreatedByUserId", "DefaultPricingTypeId", "IsActive", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, "Shirt", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, null, null },
                    { 2, "Trouser", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, null, null }
                });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryId", "CategoryName", "CreatedAt", "CreatedByUserId", "DefaultPricingTypeId", "IsActive", "IsSpecialItem", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 3, "Jacket", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, true, null, null },
                    { 4, "Kurtha", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, true, null, null },
                    { 5, "Saree", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, true, null, null }
                });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryId", "CategoryName", "CreatedAt", "CreatedByUserId", "DefaultPricingTypeId", "IsActive", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { 6, "Bedsheet", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, true, null, null });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryId", "CategoryName", "CreatedAt", "CreatedByUserId", "DefaultPricingTypeId", "IsActive", "IsSpecialItem", "UpdatedAt", "UpdatedByUserId" },
                values: new object[] { 7, "Curtain", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, true, true, null, null });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryId", "CategoryName", "CreatedAt", "CreatedByUserId", "DefaultPricingTypeId", "IsActive", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 8, "Linen", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, true, null, null },
                    { 9, "Uniform", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, null, null },
                    { 10, "Other", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, true, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcknowledgementTypes_AcknowledgementCode",
                table: "AcknowledgementTypes",
                column: "AcknowledgementCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcknowledgementTypes_AcknowledgementName",
                table: "AcknowledgementTypes",
                column: "AcknowledgementName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Action",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityId",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName",
                table: "AuditLogs",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchName",
                table: "Branches",
                column: "BranchName");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionModes_CollectionModeCode",
                table: "CollectionModes",
                column: "CollectionModeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionModes_CollectionModeName",
                table: "CollectionModes",
                column: "CollectionModeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerNo",
                table: "Customers",
                column: "CustomerNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerTypeId",
                table: "Customers",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MobileNo",
                table: "Customers",
                column: "MobileNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypes_CustomerTypeCode",
                table: "CustomerTypes",
                column: "CustomerTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionIssues_InspectionIssueTypeId",
                table: "InspectionIssues",
                column: "InspectionIssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionIssues_InspectionRecordId",
                table: "InspectionIssues",
                column: "InspectionRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionIssues_InspectionRecordId_InspectionIssueTypeId",
                table: "InspectionIssues",
                columns: new[] { "InspectionRecordId", "InspectionIssueTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionIssueTypes_IssueCode",
                table: "InspectionIssueTypes",
                column: "IssueCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionIssueTypes_IssueName",
                table: "InspectionIssueTypes",
                column: "IssueName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPhotos_CreatedByUserId",
                table: "InspectionPhotos",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPhotos_InspectionRecordId",
                table: "InspectionPhotos",
                column: "InspectionRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPhotos_UpdatedByUserId",
                table: "InspectionPhotos",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPhotos_UploadedAt",
                table: "InspectionPhotos",
                column: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPhotos_UploadedByUserId",
                table: "InspectionPhotos",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecords_CreatedByUserId",
                table: "InspectionRecords",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecords_InspectedAt",
                table: "InspectionRecords",
                column: "InspectedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecords_InspectedByUserId",
                table: "InspectionRecords",
                column: "InspectedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecords_OrderId",
                table: "InspectionRecords",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecords_OrderItemId",
                table: "InspectionRecords",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRecords_UpdatedByUserId",
                table: "InspectionRecords",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_CategoryName",
                table: "ItemCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_DefaultPricingTypeId",
                table: "ItemCategories",
                column: "DefaultPricingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedByUserId",
                table: "Notifications",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CustomerId",
                table: "Notifications",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationStatusId",
                table: "Notifications",
                column: "NotificationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_OrderId",
                table: "Notifications",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Recipient",
                table: "Notifications",
                column: "Recipient");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SentAt",
                table: "Notifications",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UpdatedByUserId",
                table: "Notifications",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStatuses_StatusCode",
                table: "NotificationStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStatuses_StatusName",
                table: "NotificationStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_TypeCode",
                table: "NotificationTypes",
                column: "TypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_TypeName",
                table: "NotificationTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderCancellations_CancelledAt",
                table: "OrderCancellations",
                column: "CancelledAt");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCancellations_CancelledByUserId",
                table: "OrderCancellations",
                column: "CancelledByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCancellations_OrderId",
                table: "OrderCancellations",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderCollections_CollectedOrDeliveredAt",
                table: "OrderCollections",
                column: "CollectedOrDeliveredAt");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCollections_CollectionModeId",
                table: "OrderCollections",
                column: "CollectionModeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCollections_CollectorMobileNo",
                table: "OrderCollections",
                column: "CollectorMobileNo");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCollections_OrderId",
                table: "OrderCollections",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderCollections_ReleasedByUserId",
                table: "OrderCollections",
                column: "ReleasedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CreatedByUserId",
                table: "OrderItems",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemCategoryId",
                table: "OrderItems",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_PricingTypeId",
                table: "OrderItems",
                column: "PricingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ServiceId",
                table: "OrderItems",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ServicePriceId",
                table: "OrderItems",
                column: "ServicePriceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_UpdatedByUserId",
                table: "OrderItems",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTags_CreatedByUserId",
                table: "OrderItemTags",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTags_OrderItemId",
                table: "OrderItemTags",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTags_TagNo",
                table: "OrderItemTags",
                column: "TagNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTags_UpdatedByUserId",
                table: "OrderItemTags",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedByUserId",
                table: "Orders",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrentStatusId",
                table: "Orders",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ExpectedReturnDate",
                table: "Orders",
                column: "ExpectedReturnDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentStatusId",
                table: "Orders",
                column: "PaymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UpdatedByUserId",
                table: "Orders",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_StatusCode",
                table: "OrderStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_StatusName",
                table: "OrderStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_ChangedAt",
                table: "OrderStatusHistories",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_ChangedByUserId",
                table: "OrderStatusHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_OrderId",
                table: "OrderStatusHistories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_OrderStatusId",
                table: "OrderStatusHistories",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_MethodCode",
                table: "PaymentMethods",
                column: "MethodCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_MethodName",
                table: "PaymentMethods",
                column: "MethodName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedByUserId",
                table: "Payments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaidAt",
                table: "Payments",
                column: "PaidAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentStatusId",
                table: "Payments",
                column: "PaymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceivedByUserId",
                table: "Payments",
                column: "ReceivedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReferenceNo",
                table: "Payments",
                column: "ReferenceNo");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpdatedByUserId",
                table: "Payments",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_StatusCode",
                table: "PaymentStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_StatusName",
                table: "PaymentStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_CreatedByUserId",
                table: "PlantProcessings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_OrderId",
                table: "PlantProcessings",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_OverallQCStatusId",
                table: "PlantProcessings",
                column: "OverallQCStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_PlantId",
                table: "PlantProcessings",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_ReadyDate",
                table: "PlantProcessings",
                column: "ReadyDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_ReceivedAtPlant",
                table: "PlantProcessings",
                column: "ReceivedAtPlant");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessings_UpdatedByUserId",
                table: "PlantProcessings",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_CompletedAt",
                table: "PlantProcessingStageUpdates",
                column: "CompletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_PlantProcessingId",
                table: "PlantProcessingStageUpdates",
                column: "PlantProcessingId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_PlantProcessingId_ProcessingSta~",
                table: "PlantProcessingStageUpdates",
                columns: new[] { "PlantProcessingId", "ProcessingStageId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_ProcessingStageId",
                table: "PlantProcessingStageUpdates",
                column: "ProcessingStageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_ProcessingStageStatusId",
                table: "PlantProcessingStageUpdates",
                column: "ProcessingStageStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_StartedAt",
                table: "PlantProcessingStageUpdates",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PlantProcessingStageUpdates_UpdatedByUserId",
                table: "PlantProcessingStageUpdates",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantName",
                table: "Plants",
                column: "PlantName");

            migrationBuilder.CreateIndex(
                name: "IX_PricingTypes_PricingTypeCode",
                table: "PricingTypes",
                column: "PricingTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStages_SortOrder",
                table: "ProcessingStages",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStages_StageCode",
                table: "ProcessingStages",
                column: "StageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStages_StageName",
                table: "ProcessingStages",
                column: "StageName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStageStatuses_StatusCode",
                table: "ProcessingStageStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStageStatuses_StatusName",
                table: "ProcessingStageStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QCRecords_OrderItemId",
                table: "QCRecords",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QCRecords_PlantProcessingId",
                table: "QCRecords",
                column: "PlantProcessingId");

            migrationBuilder.CreateIndex(
                name: "IX_QCRecords_QCStatusId",
                table: "QCRecords",
                column: "QCStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QCRecords_RecordedAt",
                table: "QCRecords",
                column: "RecordedAt");

            migrationBuilder.CreateIndex(
                name: "IX_QCRecords_RecordedByUserId",
                table: "QCRecords",
                column: "RecordedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QCStatuses_QCStatusCode",
                table: "QCStatuses",
                column: "QCStatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QCStatuses_QCStatusName",
                table: "QCStatuses",
                column: "QCStatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_ApprovedByUserId",
                table: "Refunds",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_CreatedByUserId",
                table: "Refunds",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_OrderId",
                table: "Refunds",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PaymentId",
                table: "Refunds",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_RefundedAt",
                table: "Refunds",
                column: "RefundedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_RefundStatusId",
                table: "Refunds",
                column: "RefundStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_UpdatedByUserId",
                table: "Refunds",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundStatuses_StatusCode",
                table: "RefundStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefundStatuses_StatusName",
                table: "RefundStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicePrices_ItemCategoryId",
                table: "ServicePrices",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePrices_PricingTypeId",
                table: "ServicePrices",
                column: "PricingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePrices_ServiceId_ItemCategoryId_PricingTypeId_Effect~",
                table: "ServicePrices",
                columns: new[] { "ServiceId", "ItemCategoryId", "PricingTypeId", "EffectiveFrom" });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceCode",
                table: "Services",
                column: "ServiceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceName",
                table: "Services",
                column: "ServiceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferAcknowledgements_AcknowledgedAt",
                table: "TransferAcknowledgements",
                column: "AcknowledgedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TransferAcknowledgements_AcknowledgementTypeId",
                table: "TransferAcknowledgements",
                column: "AcknowledgementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferAcknowledgements_TransferBatchId",
                table: "TransferAcknowledgements",
                column: "TransferBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferAcknowledgements_TransferBatchId_AcknowledgementTyp~",
                table: "TransferAcknowledgements",
                columns: new[] { "TransferBatchId", "AcknowledgementTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_TransferAcknowledgements_UserId",
                table: "TransferAcknowledgements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_CreatedByUserId",
                table: "TransferBatches",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_DriverUserId",
                table: "TransferBatches",
                column: "DriverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_FromBranchId",
                table: "TransferBatches",
                column: "FromBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_FromPlantId",
                table: "TransferBatches",
                column: "FromPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_ReceivedAt",
                table: "TransferBatches",
                column: "ReceivedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_SentAt",
                table: "TransferBatches",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_ToBranchId",
                table: "TransferBatches",
                column: "ToBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_ToPlantId",
                table: "TransferBatches",
                column: "ToPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_TransferNo",
                table: "TransferBatches",
                column: "TransferNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_TransferStatusId",
                table: "TransferBatches",
                column: "TransferStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_TransferTypeId",
                table: "TransferBatches",
                column: "TransferTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatches_UpdatedByUserId",
                table: "TransferBatches",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatchItems_OrderId",
                table: "TransferBatchItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatchItems_TransferBatchId",
                table: "TransferBatchItems",
                column: "TransferBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferBatchItems_TransferBatchId_OrderId",
                table: "TransferBatchItems",
                columns: new[] { "TransferBatchId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferStatuses_StatusCode",
                table: "TransferStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferStatuses_StatusName",
                table: "TransferStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferTypes_TransferTypeCode",
                table: "TransferTypes",
                column: "TransferTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferTypes_TransferTypeName",
                table: "TransferTypes",
                column: "TransferTypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBranchAssignments_BranchId",
                table: "UserBranchAssignments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranchAssignments_UserId_BranchId",
                table: "UserBranchAssignments",
                columns: new[] { "UserId", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlantAssignments_PlantId",
                table: "UserPlantAssignments",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlantAssignments_UserId_PlantId",
                table: "UserPlantAssignments",
                columns: new[] { "UserId", "PlantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultBranchId",
                table: "Users",
                column: "DefaultBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultPlantId",
                table: "Users",
                column: "DefaultPlantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "InspectionIssues");

            migrationBuilder.DropTable(
                name: "InspectionPhotos");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderCancellations");

            migrationBuilder.DropTable(
                name: "OrderCollections");

            migrationBuilder.DropTable(
                name: "OrderItemTags");

            migrationBuilder.DropTable(
                name: "OrderStatusHistories");

            migrationBuilder.DropTable(
                name: "PlantProcessingStageUpdates");

            migrationBuilder.DropTable(
                name: "QCRecords");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "TransferAcknowledgements");

            migrationBuilder.DropTable(
                name: "TransferBatchItems");

            migrationBuilder.DropTable(
                name: "UserBranchAssignments");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserPlantAssignments");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "InspectionIssueTypes");

            migrationBuilder.DropTable(
                name: "InspectionRecords");

            migrationBuilder.DropTable(
                name: "NotificationStatuses");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "CollectionModes");

            migrationBuilder.DropTable(
                name: "ProcessingStageStatuses");

            migrationBuilder.DropTable(
                name: "ProcessingStages");

            migrationBuilder.DropTable(
                name: "PlantProcessings");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RefundStatuses");

            migrationBuilder.DropTable(
                name: "AcknowledgementTypes");

            migrationBuilder.DropTable(
                name: "TransferBatches");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "QCStatuses");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "TransferStatuses");

            migrationBuilder.DropTable(
                name: "TransferTypes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ServicePrices");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "PricingTypes");
        }
    }
}
