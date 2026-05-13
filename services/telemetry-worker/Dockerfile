FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj first (for caching)
COPY services/telemetry-worker/telemetry-worker.csproj services/telemetry-worker/
COPY services/telemetry-contracts/telemetry-contracts.csproj services/telemetry-contracts/

RUN dotnet restore services/telemetry-worker/telemetry-worker.csproj

# full source
COPY . .

RUN dotnet publish services/telemetry-worker/telemetry-worker.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "telemetry-worker.dll"]
