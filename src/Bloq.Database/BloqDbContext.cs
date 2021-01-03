using Bloq.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bloq.Database
{
    public class BloqDbContext : IdentityDbContext<BloqUser, IdentityRole<Guid>, Guid>
    {
        public BloqDbContext(DbContextOptions<BloqDbContext> options) : base(options)
        {
        }
    }
}
