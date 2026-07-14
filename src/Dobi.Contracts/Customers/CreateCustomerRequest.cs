using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Customers
{
    public sealed record CreateCustomerRequest(
        string FullName,
        string MobileNo,
        string Address,
        string? Email,
        int CustomerTypeId = 1);
}
