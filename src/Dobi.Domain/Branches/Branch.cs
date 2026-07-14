using Dobi.Domain.Common;
using Dobi.Domain.Identity;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Branches
{
    public class Branch : AuditableEntity
    {
        public string BranchName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactNo { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<UserBranchAssignment> UserAssignments { get; set; } = new List<UserBranchAssignment>();
    }
}
