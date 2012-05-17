using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrackerJack.OAuth.Request;
using log4net;

namespace CrackerJack.OAuth.Validation
{
    public class ClientCredentialsRequestValidator : IRequestValidator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ClientCredentialsRequestValidator));

        public ValidationResult ValidateRequest(IOAuthRequest request)
        {
            _logger.Debug("Validating client credentials request");

            if (request.Method != HttpMethod.Post)
                return new ValidationResult
                           {ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Http POST method is required"};

            if (request.ContentType != ContentType.FormEncoded)
                return new ValidationResult
                           {
                               ErrorCode = ErrorCode.InvalidRequest,
                               ErrorDescription = "Content Type must be  application/x-www-form-urlencoded"
                           };

            var grantType = request.GrantType;
            if (string.IsNullOrWhiteSpace(grantType))
                return new ValidationResult
                           {ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter grant_type is missing"};

            if (grantType != GrantType.ClientCredentials)
                return new ValidationResult
                           {
                               ErrorCode = ErrorCode.InvalidGrant,
                               ErrorDescription = "The specified grant_type is not supported"
                           };

            var clientId = request.ClientId;
            if (string.IsNullOrWhiteSpace(clientId))
                return new ValidationResult
                           {ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter client_id is missing"};

            var clientSecret = request.ClientSecret;
            if (string.IsNullOrWhiteSpace(clientSecret))
                return new ValidationResult
                           {
                               ErrorCode = ErrorCode.InvalidRequest,
                               ErrorDescription = "Parameter client_secret is missing"
                           };

            return new ValidationResult {Success = true};
        }
    }
}
