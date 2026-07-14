using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Transfers
{
    public sealed record TransferAcknowledgementResponse(
        int TransferAcknowledgementId,
        int AcknowledgementTypeId,
        string AcknowledgementName,
        int UserId,
        DateTime AcknowledgedAt,
        string? SignatureUrl,
        string? Remarks);
}
