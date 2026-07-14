using Dobi.Domain.Branches;
using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Identity
{
    public class UserBranchAssignment : AuditableEntity
    {
        public int UserId { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
