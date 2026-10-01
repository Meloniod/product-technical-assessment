This repository contains a product dashboard with a Next.js frontend and an ASP.NET Core API. The dashboard loads products and requests a sales summary when a product is selected.

## Requirements

- .NET 8 SDK and Node.js 22 (with npm) to run the applications manually.
- Docker with the Docker Compose plugin to run the containerized applications.

## Run Tests

Run the backend test projects from the repository root:

```powershell
dotnet test .\product-backend\Api.Tests\Api.Tests.csproj
dotnet test .\product-backend\Application.Tests\Application.Tests.csproj
```

Run the frontend tests from the frontend directory:

```powershell
cd .\product-frontend
npm ci
npm test
```

To run a single frontend test file, pass its path to Vitest, for example:

```powershell
npm test -- components/ProductCard.test.tsx
```

Run the frontend linter with `npm run lint` from `product-frontend`.

## Run Manually

The manual setup uses HTTPS for the API, so trust the ASP.NET Core development certificate on your machine if you have not already:

```powershell
dotnet dev-certs https --trust
```

Start the API from the repository root:

```powershell
dotnet run --project .\product-backend\Api\Api.csproj --launch-profile https
```

The API listens at `https://localhost:7220`; Swagger is available at `https://localhost:7220/swagger`.

In a second terminal, start the frontend:

```powershell
cd .\product-frontend
npm ci
$env:NEXT_PUBLIC_API_BASE_URL = "https://localhost:7220"
npm run dev
```

Open `http://localhost:3000`. The frontend API URL is supplied through `NEXT_PUBLIC_API_BASE_URL`; the checked-in `.env.local` already points to the HTTPS API URL shown above.

## Run With Docker Compose

The base Compose configuration is the production deployment: it runs the API with `ASPNETCORE_ENVIRONMENT=Production` and the frontend with the Next.js production server. From the repository root, build and start both services:

```powershell
docker compose up --build
```

By default, open the frontend at `http://localhost:3000` and the API at `http://localhost:8080`. Swagger is enabled only when the API runs in Development. The API provides `/health`; Compose checks both services and waits for the API to become healthy before starting the frontend.

### Configuration

Set these variables in a root `.env` file before building. The file is git-ignored and Compose loads it automatically.

| Variable | Default | Purpose |
| --- | --- | --- |
| `ASPNETCORE_ENVIRONMENT` | `Production` | ASP.NET Core environment for the base Compose configuration. |
| `NEXT_PUBLIC_API_BASE_URL` | `http://localhost:8080` | Browser-accessible API URL; this value is embedded in the frontend build. |
| `FRONTEND_ORIGIN` | `http://localhost:3000` | Allowed browser origin for the API's CORS policy. |
| `API_PORT` | `8080` | Host port published for the API. The container listens on `8080`. |
| `FRONTEND_PORT` | `3000` | Host port published for the frontend. The container listens on `3000`. |

Example production `.env` for a deployment behind HTTPS reverse proxies:

```dotenv
ASPNETCORE_ENVIRONMENT=Production
NEXT_PUBLIC_API_BASE_URL=https://api.example.com
FRONTEND_ORIGIN=https://dashboard.example.com
API_PORT=8080
FRONTEND_PORT=3000
```

`NEXT_PUBLIC_API_BASE_URL` must be reachable from users' browsers. Rebuild the frontend with `docker compose up --build` after changing it. Server-rendered frontend requests use the Compose-only address `API_INTERNAL_BASE_URL=http://api:8080`; this is separate from the browser URL and normally does not need changing. Set `API_PORT` and `FRONTEND_PORT` to different host ports if the defaults are already in use.

### Development

For containerized development, the override mounts both source trees and runs `dotnet watch` and `next dev`:

```powershell
docker compose -f .\docker-compose.yml -f .\docker-compose.development.yml up --build
```

This override sets the API environment to Development and the frontend's `NODE_ENV` to `development`. For local Docker use, the default API URL and frontend origin are already set to the published localhost ports. Stop this configuration with:

```powershell
docker compose -f .\docker-compose.yml -f .\docker-compose.development.yml down
```

Stop the production configuration with `Ctrl+C`, then remove the containers with:

```powershell
docker compose down
```

## AI Assistance

ChatGPT and GitHub Copilot were used as development aids during this assessment. They helped explore implementation options, reason through API and frontend behavior, and refine code and tests. They also assisted with preparing project setup, testing, and usage documentation.

AI suggestions were treated as a starting point, not as authoritative output. I reviewed and adapted the changes to the existing architecture and requirements, and I remain responsible for the final implementation and its correctness. Test and validation status should be judged from the commands actually run and their reported results, rather than inferred from AI-generated suggestions.