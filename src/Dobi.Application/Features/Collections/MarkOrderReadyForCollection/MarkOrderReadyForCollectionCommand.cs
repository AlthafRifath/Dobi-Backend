using Dobi.Contracts.Collections;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.MarkOrderReadyForCollection
{
    public sealed record MarkOrderReadyForCollectionCommand(
        int OrderId,
        string? Remarks) : IRequest<OrderCollectionResponse>;
}
