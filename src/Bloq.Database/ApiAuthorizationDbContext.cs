using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.ApiAuthorization.IdentityServer
{
    // can't believe I gotta re-implement this whole DbContext, just so I can use a Guid instead of a string as the primary key...
    public class ApiAuthorizationDbContext<TUser, TKey> : IdentityDbContext<TUser, IdentityRole<TKey>, TKey>, IPersistedGrantDbContext
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public ApiAuthorizationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options)
        {
            _operationalStoreOptions = operationalStoreOptions;
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }
}
