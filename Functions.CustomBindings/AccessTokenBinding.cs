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

        private readonly string _clientId;

        private readonly ConfigurationManager<OpenIdConnectConfiguration> _b2cConfigurationManager;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _aadConfigurationManager;

        public AccessTokenBinding()
        {
            var b2cMetadataAddress = Environment.GetEnvironmentVariable("B2C_METADATA_ADDRESS");
            var aadMetadataAddress = Environment.GetEnvironmentVariable("AAD_METADATA_ADDRESS");
            _clientId = Environment.GetEnvironmentVariable("CLIENT_ID");

            _b2cConfigurationManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    b2cMetadataAddress, new OpenIdConnectConfigurationRetriever());

            _aadConfigurationManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    aadMetadataAddress, new OpenIdConnectConfigurationRetriever());
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

            var handler = new JwtSecurityTokenHandler();

            Exception b2cException;

            try
            {
                var connectConfiguration = await _b2cConfigurationManager.GetConfigurationAsync();

                var claimsPrincipal = handler.ValidateToken(bearerToken, new TokenValidationParameters()
                {
                    ValidAudience = _clientId,
                    ValidIssuer = connectConfiguration.Issuer,
                    IssuerSigningKeys = connectConfiguration.SigningKeys,
                    ValidateIssuerSigningKey = true
                }, out _);

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
                var connectConfiguration = await _aadConfigurationManager.GetConfigurationAsync();

                var claimsPrincipal = handler.ValidateToken(bearerToken, new TokenValidationParameters()
                {
                    ValidAudience = _clientId,
                    ValidIssuer = connectConfiguration.Issuer,
                    IssuerSigningKeys = connectConfiguration.SigningKeys,
                    ValidateIssuerSigningKey = true
                }, out _);

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
    }
}
