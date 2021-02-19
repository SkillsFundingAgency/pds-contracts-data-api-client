using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pds.Contracts.Data.Api.Client.ConfigurationOptions;
using Pds.Contracts.Data.Api.Client.Enumerations;
using Pds.Contracts.Data.Api.Client.Extensions;
using Pds.Contracts.Data.Api.Client.Implementations;
using Pds.Contracts.Data.Api.Client.Interfaces;
using Pds.Core.ApiClient.Interfaces;
using Pds.Core.ApiClient.Services;
using Pds.Core.Utils.Implementations;
using Pds.Core.Utils.Interfaces;
using Polly.Registry;

namespace Pds.Contracts.Data.Api.Client.Registrations
{
    /// <summary>
    /// Extensions class for <see cref="IServiceCollection"/> for registering the feature's services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the current feature to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to add the feature's services to.
        /// </param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="policyRegistry">
        /// The <see cref="IPolicyRegistry{TKey}"/> to add the new policies to.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddContractsDataApiClient(this IServiceCollection services, IConfiguration configuration, IPolicyRegistry<string> policyRegistry)
        {
            var policies = new PolicyType[] { PolicyType.Retry, PolicyType.CircuitBreaker };

            // Configure Polly Policies for IContractsDataService HttpClient
            services
                .AddPolicies<IContractsDataService>(configuration, policyRegistry)
                .AddHttpClientBuilder<IContractsDataService, ContractsDataService, ContractsDataApiConfiguration>(configuration, policies);

            services.AddTransient(typeof(IAuthenticationService<>), typeof(AuthenticationService<>));
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}