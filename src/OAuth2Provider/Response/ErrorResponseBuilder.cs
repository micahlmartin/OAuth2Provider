using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Response
{
    public class ErrorResponseBuilder : ResponseBuilderBase
    {
        public ErrorResponseBuilder() { SetStatusCode(400); }
        public ErrorResponseBuilder(OAuthException exception)
        {
            SetStatusCode(400);
            DisableCache();
            SetErrorCode(exception.ErrorCode);
            SetErrorDescription(exception.ErrorDescription);
            SetErrorUri(exception.ErrorUri);
            SetState(exception.State);
        }
        public ErrorResponseBuilder SetStatusCode(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public ErrorResponseBuilder SetParam(string key, object value)
        {
            Parameters.Add(key, value);
            return this;
        }

        public ErrorResponseBuilder SetLocation(string location)
        {
            Location = location;
            return this;
        }

        public ErrorResponseBuilder SetErrorCode(string error)
        {
            Parameters.Add(OAuthTokens.Error, error);
            return this;
        }

        public ErrorResponseBuilder SetErrorDescription(string description)
        {
            Parameters.Add(OAuthTokens.ErrorDescription, description);
            return this;
        }

        public ErrorResponseBuilder SetErrorUri(string uri)
        {
            Parameters.Add(OAuthTokens.ErrorUri, uri);
            return this;
        }

        public ErrorResponseBuilder SetState(string state)
        {
            Parameters.Add(OAuthTokens.State, state);
            return this;
        }

        public ErrorResponseBuilder DisableCache()
        {
            Headers.Add("Cache-Control", "no-store");
            return this;
        }
    }
}
