using Dobi.Domain.Branches;
using Dobi.Domain.Collections;
using Dobi.Domain.Common;
using Dobi.Domain.Customers;
using Dobi.Domain.Inspections;
using Dobi.Domain.Notifications;
using Dobi.Domain.Payments;
using Dobi.Domain.Transfers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Orders
{
    public class Order : AuditableEntity
    {
        public string OrderNo { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        public DateOnly? ExpectedReturnDate { get; set; }

        public int CurrentStatusId { get; set; }
        public OrderStatus CurrentStatus { get; set; } = null!;

        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = null!;

        public bool IsExpress { get; set; }

        public decimal SubTotalAmount { get; set; }
        public decimal ExpressChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
        public ICollection<InspectionRecord> InspectionRecords { get; set; } = new List<InspectionRecord>();
        public ICollection<TransferBatchItem> TransferBatchItems { get; set; } = new List<TransferBatchItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public OrderCancellation? Cancellation { get; set; }
        public PlantProcessing.PlantProcessing? PlantProcessing { get; set; }
        public OrderCollection? Collection { get; set; }
    }
}
