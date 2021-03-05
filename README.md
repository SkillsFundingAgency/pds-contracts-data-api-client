# Manage your education and skills funding - Pds.Contracts.Data.Api.Client

## Introduction

This is a client package used for contracts data API to provide a consistent and simpler access to contracs data API. This package abstracts HttpClient calls and responses and provides a c# typed response for consumption.
More information about contracts data API including details about API reference can be found at <https://github.com/SkillsFundingAgency/pds-contracts-data-api>  

### Getting Started

This is a Visual Studio 2019 project containing implementation to abstract calls to Contracts data API.
To use this package clone the project and open the solution in Visual Studio 2019 to build and package locally.

### Dependencies

This pakcage has the following dependencies.

#### Depended by (decendends depends on this package)

* None

#### Depends on (this package depends on the following)

* [Pds.Core.ApiClient](https://dev.azure.com/sfa-gov-uk/Provider%20Digital%20Services/_git/pds-packages?path=%2FPds.Core.ApiClient)
* [Pds.Core.CodeAnalysis.StyleCop](https://dev.azure.com/sfa-gov-uk/Provider%20Digital%20Services/_git/pds-packages?path=%2FPds.Core.CodeAnalysis.StyleCop)

## Usage example

To use this contracts api client sdk in your application after installation add `AddContractsDataApiClient` during startup to services collection as below.
It requires polly registry for transient fault tollerance.

```c#
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add polly registry and pass that on to the data contracts api client.
            var policyRegistry = services.AddPolicyRegistry();
            services.AddContractsDataApiClient(Configuration, policyRegistry);
        }
    }
```

Once added to services collection then constructor injection can be used to access an implementation instance of `IDataContractService` to access contract api. See example below.

```c#
    public class ContractsDataServiceExample
    {
        private readonly IContractsDataService _contractsDataClient;

        public ContractsDataServiceExample(IContractsDataService contractsDataClient)
        {
            _contractsDataClient = contractsDataClient;
        }

        public async Task AnExampleCall()
        {
            var result = await _contractsDataClient.GetContractByContractNumberAndVersionAsync(contractNumber:"Test-1234", version:1);
            return result;
        }
    }
```

## Build and Test

This packages are is built using

* Microsoft Visual Studio 2019
* .Net Core 3.1

To build and test locally, you can either use visual studio 2019 or VSCode or simply use dotnet CLI `dotnet build` and `dotnet test` more information in dotnet CLI can be found at <https://docs.microsoft.com/en-us/dotnet/core/tools/>.

## Contribute

To contribute,

* If you are part of the team then create a branch for changes and then then submit your changes for review by creating a pull request.
* If you are external to the organisation then fork this repository and make necessary changes and then submit your changes for review by creating a pull request.
