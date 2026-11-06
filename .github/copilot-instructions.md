# Copilot Instructions for the ReMoS Assurance Service API

Use these instructions when working in `nirms-exports-signup-service-api` or creating documentation about how the API interacts with the UI, Functions app, and external systems.

## Application Role

This project is the ASP.NET Core Web API behind the ReMoS sign-up UI. It owns the HTTP contract, application services, SQL persistence path, address lookup integration, and outbound Service Bus events.

It is one of three separate deployable applications in the SUS ReMoS estate:

- `nirms-exports-signup-service-ui`: user journey, page flow, validation, authentication-session handling, and calls to this API.
- `nirms-exports-signup-service-api`: HTTP API, SQL persistence, address lookup facade, and outbound Service Bus publishing.
- `nirms-exports-signup-service-functions`: inbound Service Bus status-feedback consumers that update the shared SQL database.

Document this application as the data and integration API for the estate, not as the browser journey or the downstream approval processor.

## Responsibilities

The API is responsible for:

- Exposing version 1 HTTP operations for trade parties and establishments.
- Creating, retrieving, and updating trade-party applications, including addresses, contacts, authorised signatories, submission details, approval status, and ReMoS business scheme numbers.
- Creating, retrieving, searching, sorting, paging, updating, and deleting establishments/logistics locations.
- Assigning ReMoS business and establishment scheme numbers.
- Preventing duplicate active establishments where service logic enforces it.
- Persisting state to SQL Server through Entity Framework Core repositories.
- Using the Defra Trade Address API client for postcode and UPRN lookup.
- Publishing completed sign-up and self-service change messages to Azure Service Bus when the relevant feature flags are enabled.

The API is not responsible for:

- Rendering or sequencing the browser journey.
- Authenticating end users in Razor Pages or reading organisation membership from claims.
- Performing the external approval or rejection decision.
- Consuming the downstream feedback messages that it indirectly causes.

## Important Source Areas

- Controllers: `src/Defra.Trade.ReMoS.AssuranceService.API/Controllers/`.
- Application services: `src/Defra.Trade.ReMoS.AssuranceService.API.Core/Services/`.
- Service interfaces and contracts: `src/Defra.Trade.ReMoS.AssuranceService.API.Core/Interfaces/` and `src/Defra.Trade.ReMoS.AssuranceService.API.Core/Contracts.cs`.
- Persistence and repositories: `src/Defra.Trade.ReMoS.AssuranceService.API.Data/Persistence/`.
- Entity Framework migrations and model configuration: `src/Defra.Trade.ReMoS.AssuranceService.API.Data/Persistence/Migrations/` and `Configurations/`.
- Domain entities and outbound message models: `src/Defra.Trade.ReMoS.AssuranceService.API.Domain/`.
- Shared DTO contracts: `Defra.Trade.ReMoS.AssuranceService.Shared/DTO/`.
- API Management configuration: `api-config/`.

## Interaction Model

Use this as the default interaction model unless source code proves a narrower case:

1. The UI calls this API for trade-party, establishment, and address-lookup operations.
2. Controllers accept DTOs and delegate behaviour to Core services.
3. Core services coordinate mapping, business rules, repository calls, scheme-number assignment, and optional integration publishing.
4. Repositories persist and query SQL Server through Entity Framework Core.
5. Address lookup operations call the Defra Trade Address API and return address DTOs to the UI; these lookup results are not persisted until the UI submits chosen address data.
6. Completed sign-up and self-service changes can publish Service Bus messages for downstream Trade Platform processing.
7. Downstream processes return approval/status feedback outside this API.
8. The Functions app consumes that feedback and updates the same SQL database.
9. Subsequent API reads expose the updated status to the UI.

## Integration Messages

When documenting outbound messages, include publisher, transport, trigger condition, payload subject, consumer expectation, and persistence effect.

Known message subjects and purposes include:

- `sus.remos.signup`: completed sign-up application data.
- `sus.remos.update`: self-service contact or authorised representative changes.
- `sus.remos.establishment.create`: self-service establishment creation or update payload for active locations.
- `sus.remos.establishment.update`: self-service establishment removal/status update payload.

Database persistence happens before message publication in the update flows. Existing sender helpers catch send exceptions and log them rather than failing the HTTP request, so do not document message delivery as transactionally guaranteed unless the implementation changes.

## Domain Language

- `TradeParty`: the business registration aggregate for a Defra organisation.
- `OrgId`: the Defra organisation identifier used to find the current trade party.
- `TradeAddress`: an address associated with a trade party or establishment; `TradeCountry` also drives GB dispatch versus NI destination behaviour.
- `TradeContact`: the primary business contact.
- `AuthorisedSignatory`: the person authorised to act for the business.
- `LogisticsLocation` or establishment: a place of dispatch in GB or place of destination in NI.
- `RemosBusinessSchemeNumber`: the ReMoS business identifier, such as `RMS-GB-000001`.
- `RemosEstablishmentSchemeNumber`: the establishment identifier, such as `RMS-GB-000001-001`.

Approval states used across the application include `NotSignedUp`, `SignupStarted`, `PendingApproval`, `Approved`, `Rejected`, and `Suspended` for trade parties, and `Draft`, `Approved`, `Rejected`, `Suspended`, and `Removed` for establishments.

## Documentation Guidance

- Write in UK English.
- Distinguish synchronous HTTP operations from asynchronous Service Bus publication.
- Identify whether routes are used by the UI, integration clients, or both when that can be determined from source.
- Call out API Management behaviour from `api-config/` separately from controller behaviour in source.
- Do not imply the API makes approval decisions or consumes feedback queues.
- When uncertain, cite the owning controller, service method, repository method, DTO, or configuration file to ground the explanation.
