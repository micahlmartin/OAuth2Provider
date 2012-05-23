using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public class OAuthException : Exception
    {
        public OAuthException(string errorCode, string errorDescription, string errorUri = null, string state = null) : base(errorDescription)
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
            ErrorUri = errorUri;
            State = state;
        }

        public string ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        public string ErrorUri { get; private set; }
        public string State { get; set; }
    }
}
