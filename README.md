# API Aggregation Service

## Overview

This project is an API aggregation service that fetches data from multiple external API providers simultaneously, aggregates the results, and exposes them through a REST API.

The service includes:

* Parallel fetching from multiple external APIs
* Aggregation of external API values
* In-memory caching of aggregated responses
* Request statistics tracking per external provider
* Background performance monitoring for anomaly detection
* JWT Bearer authentication
* Docker support


---

## How It Works

### External API Aggregation

The service calls multiple external providers concurrently.

Each provider:

1. Sends a request to its external API
2. Returns a normalized model
3. Records execution time and success/failure status

The results are then aggregated into a single response.

Example:

```json
{
  "dataSource": "ExternalAPI",
  "timeFetched": "2026-06-22T00:06:00Z",
  "aggregatedValue": 14395.10,
  "sourcesUsed": 5,
  "providers": [
    {
      "provider": "Bitfinex",
      "value": 65046.00
    }
  ]
}
```

---

## Caching

The service uses `IMemoryCache`.

Aggregated results are cached and reused for the current minute to avoid unnecessary external API calls.

Cache flow:

1. Request arrives
2. Check cache
3. If cached value exists, return it
4. Otherwise call external APIs
5. Aggregate result
6. Store response in cache

---

## Statistics

The service tracks:

* Total requests
* Failed requests
* Fast requests
* Average requests
* Slow requests
* Average response time

Statistics are stored in memory using a thread-safe collection.

---

## Sorting

The aggregated response supports optional sorting of provider results.

The available sorting options are based on the data returned from external providers:

### Sort by Provider

Sort providers alphabetically by provider name.

Example:

```http
GET /api/aggregation?SortBy=Provider&Direction=Asc
```

Result:

```json
[
  {
    "provider": "Bitfinex",
    "value": 65046.00
  },
  {
    "provider": "Github",
    "value": 5476.00
  }
]
```

---

### Sort by Value

Sort providers based on the returned API value.

Example:

```http
GET /api/aggregation?SortBy=value&Direction=Desc
```

Same for request's statistics Endpoint.

---

## Background Monitoring

A hosted background service periodically checks API performance statistics.

It detects anomalies based on configured thresholds.

Example:

```
External API performance anomaly detected.
Average response time: 300ms
```

---

## Authentication

The API is secured using JWT Bearer authentication.

Protected endpoints require:

```
Authorization: Bearer {token}
```

JWT validation is configured through `appsettings.json`.

---

# Running the Application

## Requirements

* .NET 8 SDK
* Docker (optional)

---

## Run Locally

Clone the repository:

```bash
git clone <repository-url>
```

Navigate to the project:

```bash
cd ApiAggregationService
```

Run the application:

```bash
dotnet run
```

The API will start and Swagger will be available at:

```
https://localhost:<port>/swagger
```

---

# Docker

## Docker Compose

Run:

```bash
docker compose up --build
```

The API will be available at:

```
http://localhost:8080
```

Stop:

```bash
docker compose down
```

---

# Testing

Run unit tests:

```bash
dotnet test
```

The tests cover:

* Aggregation logic
* Provider failures
* Statistics tracking
* Service behavior

---

# Technologies Used

* ASP.NET Core Web API
* C#
* .NET
* IMemoryCache
* JWT Authentication
* Hosted Background Services
* Docker
* xUnit
