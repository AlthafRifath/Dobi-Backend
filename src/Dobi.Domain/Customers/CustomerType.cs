using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Customers
{
    public class CustomerType : BaseEntity
    {
        public string CustomerTypeCode { get; set; } = string.Empty; // B2C, BULK
        public string CustomerTypeName { get; set; } = string.Empty;

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
