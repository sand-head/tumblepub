using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Bloq.Database.Models
{
    public class BloqUser : IdentityUser<Guid>
    {
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
