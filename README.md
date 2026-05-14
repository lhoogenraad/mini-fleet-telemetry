# Mini Fleet Telemetry System

A distributed telemetry platform built with .NET, Go, PostgreSQL, Redis, and Docker. The system simulates IoT device data ingestion and provides aggregated analytics via a separate gRPC service.

---

# What this system does

This project simulates a fleet of devices sending telemetry data (speed, battery, temperature, GPS) into a backend system that processes and aggregates the data.

## Data flow

### Real-time path
- High-speed telemetry events are routed via gRPC to a .NET ingestion worker
- These events are processed immediately

### Queue-based path
- Normal telemetry events are pushed into Redis
- A background worker processes them asynchronously

### Analytics path
- A Go-based gRPC service reads from PostgreSQL
- Computes per-device aggregates:
  - Average speed
  - Average battery
  - Average temperature
  - Total event count

---

# Architecture
Device Simulator
↓
Telemetry API (.NET)
├── Fast path → gRPC ingestion worker (.NET)
└── Slow path → Redis queue → worker (.NET)
↓
PostgreSQL
↓
Go gRPC Insights Service
↑
GET /devices/insights (API)



---

# How to run

## Prerequisites

- Docker + Docker Compose
- .NET 8 SDK (optional for local development)
- Go 1.25+ (only if modifying Go service)
- protoc (only if modifying protobufs)

---

## Start the system

From the repository root:

```bash
docker-compose up --build
```
