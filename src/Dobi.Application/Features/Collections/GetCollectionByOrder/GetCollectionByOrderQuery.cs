using Dobi.Contracts.Collections;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollectionByOrder
{
    public sealed record GetCollectionByOrderQuery(
        int OrderId) : IRequest<OrderCollectionResponse>;
}
