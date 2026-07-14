using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Transfers
{
    public class TransferType : BaseEntity
    {
        public string TransferTypeCode { get; set; } = string.Empty; // OUTLET_TO_PLANT, PLANT_TO_OUTLET
        public string TransferTypeName { get; set; } = string.Empty;

        public ICollection<TransferBatch> TransferBatches { get; set; } = new List<TransferBatch>();
    }
}
