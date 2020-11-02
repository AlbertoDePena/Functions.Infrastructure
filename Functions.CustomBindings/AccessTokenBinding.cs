using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Functions.CustomBindings
{
    public class AccessTokenBinding : IBinding
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        private readonly string _b2cMetadataAddress;
        private readonly string _aadMetadataAddress;
        private readonly string _clientId;

        private TokenValidationParameters _b2cTokenValidationParameters;
        private TokenValidationParameters _aadTokenValidationParameters;

        public AccessTokenBinding()
        {
            // TODO: validate value existance
            _b2cMetadataAddress = Environment.GetEnvironmentVariable("B2C_METADATA_ADDRESS");
            _aadMetadataAddress = Environment.GetEnvironmentVariable("AAD_METADATA_ADDRESS");
            _clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var httpRequest = context.BindingData.ContainsKey("request") ?
                context.BindingData["request"] as HttpRequest :
                context.BindingData["req"] as HttpRequest;

            return Task.FromResult<IValueProvider>(
                new AccessTokenValueProvider(ValidateTokenAsync, httpRequest));
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => Task.FromResult<IValueProvider>(null);

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();

        private async Task<AccessTokenResult> ValidateTokenAsync(HttpRequest request)
        {
            string bearerToken = string.Empty;

            if (request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                   request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
            {
                bearerToken = request.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);
            }

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return AccessTokenResult.NoToken();
            }

            await SetTokenValidationParametersAsync();

            var handler = new JwtSecurityTokenHandler();

            Exception b2cException;

            try
            {
                var claimsPrincipal = handler.ValidateToken(bearerToken, _b2cTokenValidationParameters, out _);

                return AccessTokenResult.Success(claimsPrincipal);
            }
            catch (SecurityTokenExpiredException)
            {
                return AccessTokenResult.Expired();
            }
            catch (Exception ex)
            {
                b2cException = ex;
            }

            Exception aadException;

            try
            {
                var claimsPrincipal = handler.ValidateToken(bearerToken, _aadTokenValidationParameters, out _);

                return AccessTokenResult.Success(claimsPrincipal);
            }
            catch (SecurityTokenExpiredException)
            {
                return AccessTokenResult.Expired();
            }
            catch (Exception ex)
            {
                aadException = ex;
            }

            return AccessTokenResult.Error(b2cException, aadException);
        }

        private async Task SetTokenValidationParametersAsync()
        {
            // TODO: bust cache every x minutes
            if (_b2cTokenValidationParameters == null)
            {
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(_b2cMetadataAddress, new OpenIdConnectConfigurationRetriever());
                var connectConfiguration = await configurationManager.GetConfigurationAsync();

                _b2cTokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = _clientId,
                    ValidIssuer = connectConfiguration.Issuer,
                    IssuerSigningKeys = connectConfiguration.SigningKeys,
                    ValidateIssuerSigningKey = true
                };
            }

            // TODO: bust cache every x minutes
            if (_aadTokenValidationParameters == null)
            {
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(_aadMetadataAddress, new OpenIdConnectConfigurationRetriever());
                var connectConfiguration = await configurationManager.GetConfigurationAsync();

                _aadTokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = _clientId,
                    ValidIssuer = connectConfiguration.Issuer,
                    IssuerSigningKeys = connectConfiguration.SigningKeys,
                    ValidateIssuerSigningKey = true
                };
            }
        }
    }
}
