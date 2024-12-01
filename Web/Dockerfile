FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
    
# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../Web/*.csproj ./Web/
COPY ../Application/*.csproj ./Application/
COPY ../Domain/*.csproj ./Domain/
COPY ../Infrastructure/*.csproj ./Infrastructure/
COPY ../Presentation/*.csproj ./Presentation/
COPY ../Tests/Application.Tests/*.csproj ./Tests/Application.Tests/
COPY ../Tests/Presentation.Tests/*.csproj ./Tests/Presentation.Tests/
RUN dotnet restore ./Web/Web.csproj
    
# Copy everything else and build
COPY ../Web/. ./Web/
COPY ../Application/. ./Application/
COPY ../Domain/. ./Domain/
COPY ../Infrastructure/. ./Infrastructure/
COPY ../Presentation/. ./Presentation/
COPY ../Tests/Application.Tests/. ./Tests/Application.Tests/
COPY ../Tests/Presentation.Tests/. ./Tests/Presentation.Tests/
WORKDIR /source/Web
RUN dotnet publish -c release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Web.dll"]