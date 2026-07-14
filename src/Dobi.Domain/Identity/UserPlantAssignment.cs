using Dobi.Domain.Common;
using Dobi.Domain.Plants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Identity
{
    public class UserPlantAssignment : AuditableEntity
    {
        public int UserId { get; set; }

        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;

        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
