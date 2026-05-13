FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj first (for caching)
COPY services/telemetry-api/telemetry-api.csproj services/telemetry-api/
COPY services/telemetry-contracts/telemetry-contracts.csproj services/telemetry-contracts/

RUN dotnet restore services/telemetry-api/telemetry-api.csproj

# full source
COPY . .

RUN dotnet publish services/telemetry-api/telemetry-api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "telemetry-api.dll"]
