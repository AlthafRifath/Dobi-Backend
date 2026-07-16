using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dobi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChequeAndPartialPaymentSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "Refunds",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RejectedByUserId",
                table: "Refunds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Refunds",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChequeBankName",
                table: "Payments",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChequeClearedAt",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ChequeDate",
                table: "Payments",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChequeNo",
                table: "Payments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChequeRejectedAt",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChequeRejectionReason",
                table: "Payments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodId", "MethodCode", "MethodName" },
                values: new object[] { 4, "CHEQUE", "Cheque" });

            migrationBuilder.AlterColumn<string>(
                name: "StatusCode",
                table: "PaymentStatuses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "PaymentStatuses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "PaymentStatusId", "StatusCode", "StatusName" },
                values: new object[,]
                {
                    { 5, "PENDING_CLEARANCE", "Pending Clearance" },
                    { 6, "PARTIALLY_PAID", "Partially Paid" },
                    { 7, "PARTIALLY_PAID_PENDING_CLEARANCE", "Partially Paid - Pending Clearance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "PaymentStatusId",
                keyValue: 7);

            migrationBuilder.AlterColumn<string>(
                name: "StatusCode",
                table: "PaymentStatuses",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "PaymentStatuses",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "RejectedByUserId",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "ChequeBankName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ChequeClearedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ChequeDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ChequeNo",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ChequeRejectedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ChequeRejectionReason",
                table: "Payments");
        }
    }
}
