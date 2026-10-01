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

From the repository root, build and start both services:

```powershell
docker compose up --build
```

Open the frontend at `http://localhost:3000`. The API is published at `http://localhost:8080`, with Swagger at `http://localhost:8080/swagger`. The Compose build sets `NEXT_PUBLIC_API_BASE_URL` to the host-accessible API URL because API requests are made by the user's browser.

Docker Compose serves both services over HTTP, so this setup does not require or mount an HTTPS certificate. Use the manual setup above when you want HTTPS locally.

Stop the services with `Ctrl+C`, then remove the containers with:

```powershell
docker compose down
```

## AI Assistance

ChatGPT and GitHub Copilot were used as development aids during this assessment. They helped explore implementation options, reason through API and frontend behavior, and refine code and tests. They also assisted with preparing project setup, testing, and usage documentation.

AI suggestions were treated as a starting point, not as authoritative output. I reviewed and adapted the changes to the existing architecture and requirements, and I remain responsible for the final implementation and its correctness. Test and validation status should be judged from the commands actually run and their reported results, rather than inferred from AI-generated suggestions.