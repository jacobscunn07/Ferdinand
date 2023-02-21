# Ferdinand
Ferdinand is a reference architecture for modern `dotnet` development.

:warning: This repository is in early stages of development, so the codebase and documentation will improve over time.

## Contents
1. Architecture
   1. Deployable Services
      1. Api
      2. Jobs
      3. Messaging
      4. Data Migrations
   2. Application
   3. Data
   4. Domain
2. Infrastructure
   1. Helm Chart
3. Local Development

## Architecture
The overall architecture used is Domain Driven Design (DDD) in combination with Command Query Response Separation (CQRS).

### Deployable Services
Each deployable service has very minimal application business logic. This logic has been pushed down into the Application Layer, so that each deployable service can share business logic where needed.

#### Api
A REST Api built on top of ASP.NET Core MVC. A few features to highlight are
* Versioned endpoints
* Exception handling middleware which handles transforming exceptions into a result containing the errors that consumers can easily view and quickly understand the reason for the request failure
* Transactional Outbox Pattern is implemented by default to atomically update an aggregate and publish messages

#### Jobs
:construction: This service is still to be developed. An initial implementation should be done soon.

The Jobs service is a background service that executes scheduled jobs. The initial job to be developed will be an outbox message publishing job to fully implement the Outbox Pattern.

#### Messaging
:construction: This service is still to be developed. An initial implementation should be done soon.

The Messaging service is a background service that consumes messages from a message broker.

#### Data Migrations
It is a best practice to not execute data migrations as part of your application's start up process. Doing so can lead to several problems including slow start up times and concurrency issues. The Data Migrations service is a small service whose only job is to execute data migrations and exit. 

### Application
* MediatR
  * Command / Query Handlers
  * Behaviors
* Fluent Validation

### Data
* Entity Framework Core
  * Migrations
  * Domain Model Mappings

### Domain
* Domain Driven Design (DDD)

## Infrastructure

### Helm Chart

## Local Development

Create a directory to be used for any sort of application data that will need to be persisted.
```shell
mkdir ~/.ferdinand
```

Add to your `.zhsrc` or equivalent file.
```shell
export FERDINAND_DOCKER_VOLUMES_ROOT=~/.ferdinand
```

Bring up containers
```shell
docker compose up -d web
```

Navigate to http://localhost:5000/swagger

### Entity Framework Core

#### Migrations

```shell
dotnet ef migrations add Initial \
-s src/Ferdinand.Data.Migrations \
-p src/Ferdinand.Data \
-o EntityFrameworkCore/Migrations
```

## Roadmap
Below is a list of high level objectives, in no particular order, that this repository intends to showcase.
* Deployable Services
  * Jobs Service
  * Messaging Service
  * Frontend (VueJS / Blazor)
  * AWS Lambda Function
  * Azure Function
* Data Access
  * Data Caching
  * Dapper
  * FluentMigrator
  * NoSQL
* Application Infrastructure
  * Health liveliness and readiness checks
  * Authentication / Authorization
  * Row Level Security (OPA)
  * Add problem details to API error responses to adhere to [RFC 7807](https://www.rfc-editor.org/rfc/rfc7807)
  * Options Validation (App Settings)
* Infrastructure
  * Helm Chart
  * Container Security Scanning
* Code Quality
  * Roslynator / Static Code Rules
  * SonarQube / Codacy
