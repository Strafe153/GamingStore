# GamingStore
### ASP.NET Core REST API serving as an online shop of various types of gaming peripherals.

## What Can It Do?
* Authentication and authorization
* Users administration
* Companies manipulation
* Devices management

## Dependencies
* `AspNetCore.HealthChecks.AzureStorage` for Azure Blob Storage health checks
* `AspNetCore.HealthChecks.Redis` for Redis health checks
* `AspNetCore.HealthChecks.SqlServer` for PostgreSQL health check
* `AspNetCore.HealthChecks.UI.Client` for detailed health checks information
* `AutoFixture` for test fixtures
* `AutoFixture.AutoMoq` for Moq support with AutoFixture
* `AutoMapper` for DTO mapping
* `AutoMapper.Extensions.Microsoft.DependencyInjection` for DI with AutoMapper
* `Azure.Storage.Blobs` for images storing in Azure Blob Storage
* `Bogus` for fake data generation
* `FluentAssertions` for assertions
* `FluentValidation` for DTO validation
* `IdentityServer4` for OpenID Connect and OAuth 2.0
* `IdentityServer4.AspNetIdentity` for ASP.NET Core integration with IdentityServer4
* `IdentityServer4.EntityFramework` for EF Core persistence layer for IdentityServer4
* `MediatR` for MediatR
* `MediatR.Extensions.Microsoft.DependencyInjection` for MediatR extensions for ASP.NET Core
* `Microsoft.AspNetCore.Authentication.JwtBearer` for JWT authentication
* `Microsoft.AspNetCore.Http.Abstractions` for object model for HTTP requests and responses
* `Microsoft.AspNetCore.Http.Features` for HTTP feature interface definitions
* `Microsoft.AspNetCore.Identity.EntityFrameworkCore` for identity provider that uses EF Core
* `Microsoft.AspNetCore.Mvc.Core` for core MVC components
* `Microsoft.AspNetCore.Mvc.NewtonsoftJson` for MVC features that use Newtonsoft.Json
* `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer` for API versioning
* `Microsoft.EntityFrameworkCore` for entity mapping
* `Microsoft.EntityFrameworkCore.Design` for shared design-time components for EF Core tools
* `Microsoft.EntityFrameworkCore.SqlServer` for DB context and entity mapping
* `Microsoft.EntityFrameworkCore.Tools` for EF Core tooling
* `Microsoft.Extensions.Azure` for Azure Client SDK integration
* `Microsoft.Extensions.Caching.StackExchangeRedis` for Redis distributed cache implementation
* `Microsoft.Extensions.Configuration.Abstractions` for configuration abstractions
* `Microsoft.Extensions.Configuration.UserSecrets` for user secrets provider
* `Microsoft.Extensions.DependencyInjection.Abstractions` for DI abstractions
* `Microsoft.Extensions.Identity.Stores` for Identity membership system
* `Microsoft.IdentityModel.Tokens` for security tokens
* `Microsoft.NET.Test.Sdk` for .NET SDK for testing
* `Moq` for mocking
* `Newtonsoft.Json` for JSON serialization
* `Serilog.AspNetCore` for Serilog support for ASP.NET Core
* `Serilog.Enrichers.Environment` for enricihing the Serilog configuration with Environment properties
* `Serilog.Exceptions` for detailed Serilog exceptions
* `Serilog.Sinks.Console` for Serilog console sink
* `Serilog.Sinks.Elasticsearch` for Serilog Elasticsearch sink
* `Serilog.Sinks.File` for Serilog file sink
* `SixLabors.ImageSharp` for images manipulation
* `System.IdentityModel.Tokens.Jwt` for JWT
* `Swashbuckle.AspNetCore` for Swagger support
* `xunit` for unit-tests
* `xunit.runner.visualstudio` for running tests in Visual Studio

## How To Use?
1. Sign Up
2. Log In
3. Try things out

> **NOTE!** To see advanced features, log in with "admin" as username and "qwerty" as password.
