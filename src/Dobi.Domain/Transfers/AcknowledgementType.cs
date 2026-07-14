using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Transfers
{
    public class AcknowledgementType : BaseEntity
    {
        public string AcknowledgementCode { get; set; } = string.Empty; // DRIVER_PICKUP, PLANT_RECEIVE
        public string AcknowledgementName { get; set; } = string.Empty;

        public ICollection<TransferAcknowledgement> TransferAcknowledgements { get; set; } = new List<TransferAcknowledgement>();
    }
}
