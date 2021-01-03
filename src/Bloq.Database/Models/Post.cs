using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloq.Database.Models
{
    public class Post
    {
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
