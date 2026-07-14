using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Orders
{
    public class OrderItemTag : AuditableEntity
    {
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;

        public string TagNo { get; set; } = string.Empty;
        public int? PieceNo { get; set; }
    }
}
