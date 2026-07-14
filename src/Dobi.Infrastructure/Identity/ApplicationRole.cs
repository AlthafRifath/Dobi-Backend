using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
