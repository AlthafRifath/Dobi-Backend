using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Collections
{
    public sealed record MarkOrderReadyForCollectionRequest(
        string? Remarks);
}
