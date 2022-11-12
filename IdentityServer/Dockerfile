# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
    
# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../Web/*.csproj ./Web/
COPY ../IdentityServer/*.csproj ./IdentityServer/
COPY ../Application/*.csproj ./Application/
COPY ../Domain/*.csproj ./Domain/
COPY ../Infrastructure/*.csproj ./Infrastructure/
COPY ../Presentation/*.csproj ./Presentation/
COPY ../Tests/Application.Tests/*.csproj ./Tests/Application.Tests/
COPY ../Tests/Web.Tests/*.csproj ./Tests/Web.Tests/
RUN dotnet restore
    
# Copy everything else and build
COPY ../IdentityServer/. ./IdentityServer/
COPY ../Application/. ./Application/
COPY ../Domain/. ./Domain/
COPY ../Infrastructure/. ./Infrastructure/
COPY ../Presentation/. ./Presentation/
WORKDIR /app/IdentityServer
RUN dotnet publish -c Release -o out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/IdentityServer/out .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]