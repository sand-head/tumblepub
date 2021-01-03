using System.Collections.Generic;

namespace Bloq.Shared.Models
{
    public record UserModel(
        string Username,
        bool IsAuthenticated,
        Dictionary<string, string> Claims);
}
