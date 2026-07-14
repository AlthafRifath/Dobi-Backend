using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Transfers
{
    public sealed record AcknowledgeTransferRequest(
        int AcknowledgementTypeId,
        string? SignatureUrl,
        string? Remarks);
}
