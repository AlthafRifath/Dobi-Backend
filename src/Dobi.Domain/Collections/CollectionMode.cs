using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Collections
{
    public class CollectionMode : BaseEntity
    {
        public string CollectionModeCode { get; set; } = string.Empty; // COLLECTION, DELIVERY
        public string CollectionModeName { get; set; } = string.Empty;

        public ICollection<OrderCollection> OrderCollections { get; set; } = new List<OrderCollection>();
    }
}
