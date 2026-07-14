using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Customers
{
    public sealed record UpdateCustomerRequest(
        string FullName,
        string MobileNo,
        string Address,
        string? Email,
        bool IsActive);
}
