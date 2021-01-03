using System;
using System.Collections.Generic;

namespace Bloq.Shared.Exceptions
{
    public class UserRegistrationException : Exception
    {
        public UserRegistrationException(IEnumerable<string> errors)
            : base($"The following error(s) occurred during registration:\n{string.Join('\n', errors)}")
        {
            Errors = errors;
        }

        public readonly IEnumerable<string> Errors;
    }
}
