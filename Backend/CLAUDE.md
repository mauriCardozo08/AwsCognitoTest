# Backend conventions

Monolithic .NET 10 backend following Clean Architecture.

## Layers and dependencies

- `API -> Application, Infrastructure` · `Infrastructure -> Application, Domain` · `Application -> Domain`. Domain depends on nothing.
- **Domain**: entities, value objects, domain exceptions. No HTTP, no EF Core.
- **Application**: services (`I<X>Service`), repository abstractions (`I<X>Repository`), DTOs, application exceptions, `.resx` error messages. No EF Core, no HTTP.
- **Infrastructure**: EF Core DbContext (`AppDbContext`), `IEntityTypeConfiguration`s, repository implementations.
- **API**: controllers, HTTP models, exception handler, Swagger, `Program.cs`.
- Each layer registers its services in an `Add<Layer>Module()` extension, composed in `Program.cs`. Register new services there, not in `Program.cs`.

## Naming

- Application DTOs end in `DTO` (`SampleDTO`, `CreateSampleRequestDTO`), grouped by feature under `DTOs/<Feature>/`.
- API models end in `HttpRequest` / `HttpResponse`. Controllers map between HTTP models and DTOs; never expose DTOs or entities directly in requests.
- Mapping is manual (private methods or extension methods). Do not add AutoMapper.
- User-facing error messages go in `Application/Resources/ErrorMessages.resx` (update the `.Designer.cs` too), not as string literals.

## Errors

- Every expected error derives from `AppException` with an `ErrorKind` and a stable `Code`:
  `class OrderNotFoundException(string m) : AppException(ErrorKind.NotFound, "order.not_found", m);`
- `AppExceptionHandler` is the only place that maps `ErrorKind` to HTTP status. Adding an exception must not require editing it; only a new `ErrorKind` does.
- Never put HTTP status codes in Domain or Application.
- Non-`AppException` errors become a generic 500 and are logged; do not leak their messages.

## Data

- Migrations (run from `Backend/`):
  `dotnet ef migrations add <Name> -p *.Infrastructure -s *.API -o Database/App/Migrations`
  `dotnet ef database update -p *.Infrastructure -s *.API`
- Value objects are persisted with `HasConversion` in the entity configuration.
- Connection string key is `ConnectionStrings:Default`.

## Build

- Package versions are centralized in `Directory.Packages.props`; `.csproj` files reference packages without versions.
- Shared settings (target framework, nullable, implicit usings) live in `Directory.Build.props`.
- The `Sample` feature is a placeholder example; replace it with the real domain.
