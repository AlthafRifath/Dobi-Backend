using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.PlantProcessing
{
    public class QCStatus : BaseEntity
    {
        public string QCStatusCode { get; set; } = string.Empty; // PASSED, FAILED
        public string QCStatusName { get; set; } = string.Empty;

        public ICollection<PlantProcessing> PlantProcessings { get; set; } = new List<PlantProcessing>();
        public ICollection<QCRecord> QCRecords { get; set; } = new List<QCRecord>();
    }
}
