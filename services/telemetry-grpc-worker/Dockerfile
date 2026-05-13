FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY services/telemetry-grpc-worker/telemetry-grpc-worker.csproj services/telemetry-grpc-worker/
COPY services/telemetry-contracts/telemetry-contracts.csproj services/telemetry-contracts/

RUN dotnet restore services/telemetry-grpc-worker/telemetry-grpc-worker.csproj

COPY . .

RUN dotnet publish services/telemetry-grpc-worker/telemetry-grpc-worker.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "telemetry-grpc-worker.dll"]
