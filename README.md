This repository contains a product dashboard with a Next.js frontend and an ASP.NET Core API. The dashboard loads products and requests a sales summary when a product is selected.

## Requirements

- .NET 8 SDK and Node.js 22 (with npm) to run the applications manually.
- Docker with the Docker Compose plugin to run the containerized applications.

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