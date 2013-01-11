using System;
using System.Collections.Generic;
using System.Web;
using OAuth2Provider.Validation;
using log4net;
using OAuth2Provider;

namespace OAuth2Provider.Request
{
    using System.Linq;

    public abstract class OAuthRequestBase : IOAuthRequest
    {
        private readonly IRequest _request;
        private IList<string> _requiredTokens = new List<string>();
        protected readonly ILog Logger = LogManager.GetLogger(typeof(TokenRequest));

        protected OAuthRequestBase(IRequest request, IOAuthServiceLocator serviceLocator)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (serviceLocator == null)
                throw new ArgumentNullException("serviceLocator");

            _request = request;
            ServiceLocator = serviceLocator;

            Validate();
        }

        private void Validate()
        {
            var validator = GetRequestValidator();

            var result = validator.ValidateRequest(this);
            if (!result.Success)
                throw new OAuthException(result.ErrorCode, result.ErrorDescription, result.ErrorUri);
        }

        protected abstract IRequestValidator GetRequestValidator();

        protected IOAuthServiceLocator ServiceLocator { get; set; }

        public string GrantType
        {
            get { return _request.Values.SafeGet(OAuthTokens.GrantType).FirstOrDefaultSafe(); }
        }
        public string Username
        {
            get { return _request.Values.SafeGet(OAuthTokens.Username).FirstOrDefaultSafe(); }
        }
        public string Password
        {
            get { return _request.Values.SafeGet(OAuthTokens.Password).FirstOrDefaultSafe(); }
        }
        public string Scope
        {
            get { return _request.Values.SafeGet(OAuthTokens.Scope).FirstOrDefaultSafe(); }
        }
        public string AccessToken
        {
            get
            {
                var accessToken = _request.Values.SafeGet(OAuthTokens.AccessToken).FirstOrDefaultSafe();
                if (!string.IsNullOrWhiteSpace(accessToken))
                    return accessToken;

                accessToken = _request.Values.SafeGet(OAuthTokens.OAuthToken).FirstOrDefaultSafe();
                if (!string.IsNullOrWhiteSpace(accessToken))
                    return accessToken;

                var authHeader = _request.Headers.SafeGet(HeaderType.Authorization).FirstOrDefaultSafe() ?? string.Empty;
                if (authHeader.Contains("OAuth") || authHeader.Contains("Bearer"))
                    accessToken = authHeader.Replace("OAuth ", string.Empty).Replace("Bearer ", string.Empty).Trim();

                return accessToken;
            }
        }
        public string TokenType
        {
            get { return _request.Values.SafeGet(OAuthTokens.TokenType).FirstOrDefaultSafe(); }
        }
        public string ExpiresIn
        {
            get { return _request.Values.SafeGet(OAuthTokens.ExpiresIn).FirstOrDefaultSafe(); }
        }
        public string RefreshToken
        {
            get { return _request.Values.SafeGet(OAuthTokens.RefreshToken).FirstOrDefaultSafe(); }
        }
        public string ResponseType
        {
            get { return _request.Values.SafeGet(OAuthTokens.ResponseType).FirstOrDefaultSafe(); }
        }
        public string RedirectUri
        {
            get { return _request.Values.SafeGet(OAuthTokens.RedirectUri).FirstOrDefaultSafe(); }
        }
        public string State
        {
            get { return _request.Values.SafeGet(OAuthTokens.State).FirstOrDefaultSafe(); }
        }
        public string ClientId
        {
            get
            {
                var clientID = _request.Values.SafeGet(OAuthTokens.ClientId).FirstOrDefaultSafe();
                if(!string.IsNullOrWhiteSpace(clientID))
                    return clientID;

                return BasicAuthenticationScheme.Username;
            }
        }
        public string ClientSecret
        {
            get
            {

                var clientID = _request.Values.SafeGet(OAuthTokens.ClientSecret).FirstOrDefaultSafe();
                if (!string.IsNullOrWhiteSpace(clientID))
                    return clientID;

                return BasicAuthenticationScheme.Password;
            }
        }
        public string ContentType
        {
            get { return _request.ContentType; }
        }
        public string AuthorizationCode
        {
            get { return _request.Values.SafeGet(OAuthTokens.Code).FirstOrDefaultSafe(); }
        }
        public string Method
        {
            get { return _request.HttpMethod; }
        }
        public bool IsSecure
        {
            get { return _request.IsSecure; }
        }

        private HttpBasicAuthenticationScheme _basicAuthenticationScheme;
        private HttpBasicAuthenticationScheme BasicAuthenticationScheme
        {
            get
            {
                if (_basicAuthenticationScheme == null)
                    _basicAuthenticationScheme = _request.DecodeBasicAuthentication();

                return _basicAuthenticationScheme;
            }
        }
    }
}