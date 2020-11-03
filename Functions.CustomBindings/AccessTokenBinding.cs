using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
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

        private readonly string _b2cClientId;
        private readonly string _aadClientId;

        private readonly ConfigurationManager<OpenIdConnectConfiguration> _b2cConfigurationManager;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _aadConfigurationManager;

        public AccessTokenBinding(AccessTokenAttribute attribute, INameResolver nameResolver)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (nameResolver == null) throw new ArgumentNullException(nameof(nameResolver));

            _b2cClientId = nameResolver.ResolveWholeString(attribute.B2cClientId);
            _aadClientId = nameResolver.ResolveWholeString(attribute.AadClientId);

            _b2cConfigurationManager =
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    nameResolver.ResolveWholeString(attribute.B2cMetadataAddress), new OpenIdConnectConfigurationRetriever());

            _aadConfigurationManager =
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    nameResolver.ResolveWholeString(attribute.AadMetadataAddress), new OpenIdConnectConfigurationRetriever());
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
                    ValidAudience = _b2cClientId,
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
                    ValidAudience = _aadClientId,
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
