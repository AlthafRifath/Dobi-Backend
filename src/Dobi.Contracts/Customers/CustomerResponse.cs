using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Customers
{
    public sealed record CustomerResponse(
        int CustomerId,
        string CustomerNo,
        int CustomerTypeId,
        string CustomerTypeName,
        string FullName,
        string MobileNo,
        string Address,
        string? Email,
        bool IsActive,
        DateTime CreatedAt);
}
