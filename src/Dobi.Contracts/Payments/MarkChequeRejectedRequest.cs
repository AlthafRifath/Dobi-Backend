using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record MarkChequeRejectedRequest(
        string RejectionReason);
}
