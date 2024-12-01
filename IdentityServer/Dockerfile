# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /source
    
# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../Web/*.csproj ./Web/
COPY ../IdentityServer/*.csproj ./IdentityServer/
COPY ../Application/*.csproj ./Application/
COPY ../Domain/*.csproj ./Domain/
COPY ../Infrastructure/*.csproj ./Infrastructure/
COPY ../Presentation/*.csproj ./Presentation/
COPY ../Tests/Application.Tests/*.csproj ./Tests/Application.Tests/
COPY ../Tests/Presentation.Tests/*.csproj ./Tests/Presentation.Tests/
RUN dotnet restore
    
# Copy everything else and build
COPY ../IdentityServer/. ./IdentityServer/
COPY ../Application/. ./Application/
COPY ../Domain/. ./Domain/
COPY ../Infrastructure/. ./Infrastructure/
# COPY ../Presentation/. ./Presentation/
WORKDIR /source/IdentityServer
RUN dotnet publish -c release -o /app
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]