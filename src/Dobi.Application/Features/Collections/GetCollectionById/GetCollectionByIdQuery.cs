using Dobi.Contracts.Collections;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollectionById
{
    public sealed record GetCollectionByIdQuery(
        int OrderCollectionId) : IRequest<OrderCollectionResponse>;
}
