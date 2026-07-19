# Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /source

COPY ["src/Dobi.Api/Dobi.Api.csproj", "src/Dobi.Api/"]
COPY ["src/Dobi.Application/Dobi.Application.csproj", "src/Dobi.Application/"]
COPY ["src/Dobi.Infrastructure/Dobi.Infrastructure.csproj", "src/Dobi.Infrastructure/"]
COPY ["src/Dobi.Domain/Dobi.Domain.csproj", "src/Dobi.Domain/"]
COPY ["src/Dobi.Contracts/Dobi.Contracts.csproj", "src/Dobi.Contracts/"]
COPY ["src/Dobi.Shared/Dobi.Shared.csproj", "src/Dobi.Shared/"]

RUN dotnet restore "src/Dobi.Api/Dobi.Api.csproj"

COPY . .

WORKDIR /source/src/Dobi.Api

RUN dotnet publish "Dobi.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false


# Run the published application
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

COPY --from=build /app/publish .

USER app

ENTRYPOINT ["dotnet", "Dobi.Api.dll"]