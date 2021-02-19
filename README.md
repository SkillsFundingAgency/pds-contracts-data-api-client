# Manage your education and skills funding - Contracts specific nuget packages

## Introduction

This repo contains nuget packages that are specific to contracts stream within "Manage your education and skills funding". This is a BETA version and is currently used only by a limited number of components.

### Getting Started

This is a Visual Studio 2019 solution containing a number of packages (with associated unit test and integration test projects).
To use these packages, simply clone the project and open the solution in Visual Studio 2019.

## Packages list

The following packages are available in this repo, check individual package for usage information.

* [Pds.Contracts.Data.Api.Client](/Pds.Contracts.Data.Api.Client/README.MD)

## Build and Test

This packages are is built using

* Microsoft Visual Studio 2019
* .Net Core 3.1

To build and test locally, you can either use visual studio 2019 or VSCode or simply use dotnet CLI `dotnet build` and `dotnet test` more information in dotnet CLI can be found at https://docs.microsoft.com/en-us/dotnet/core/tools/.

## Contribute

Contributions are only accepted within team at this stage.

### Versioning

We follow [SemVer](https://semver.org/) version strategy, when there is change in package then versions of all the dependent packages must also be updated, please ensure you walk down the tree and update version numbers.

All dependencies will be listed in individual packages, ensure the version as well as package release notes are updated in the csproj. 

e.g.

```xml
<PropertyGroup>
    <Description>Desciption of package, change if required mainly on a breaking change this may needs to be changed.</Description>
    <Version>1.0.0</Version>
    <PackageReleaseNotes>Created nameof-package package.</PackageReleaseNotes>
</PropertyGroup>
```
