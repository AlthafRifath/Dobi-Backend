using Dobi.Domain.Common;
using Dobi.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Plants
{
    public class Plant : AuditableEntity
    {
        public string PlantName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? OperatingHours { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserPlantAssignment> UserAssignments { get; set; } = new List<UserPlantAssignment>();
        public ICollection<PlantProcessing.PlantProcessing> PlantProcessings { get; set; } = new List<PlantProcessing.PlantProcessing>();
    }
}
