using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloq.Shared.Models.Api
{
    public record RegisterResponse(bool Success, IEnumerable<string> Errors);

    public record LoginResponse();
}
