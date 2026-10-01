# Product Dashboard Frontend

This Next.js application displays the product catalog and requests a sales summary when a product is selected. It shows loading, service-error, and product-not-found states for the summary request.

## Requirements

- Node.js 22 and npm
- The product backend running locally to use the dashboard

## Install

From this directory, install the locked dependencies:

```powershell
npm ci
```

## Run Locally

Start the backend using the instructions in the repository-root README. The HTTPS launch profile serves the API at `https://localhost:7220`.

Set the API base URL and start the frontend from this directory:

```powershell
$env:NEXT_PUBLIC_API_BASE_URL = "https://localhost:7220"
npm run dev
```

Open [http://localhost:3000](http://localhost:3000). The API base URL must be available when the app is built or started because the browser uses it for product and sales-summary requests.

## Test

Frontend tests use Vitest and mock network requests. They are self-contained: no running backend or `NEXT_PUBLIC_API_BASE_URL` is required.

Run the full test suite from this directory:

```powershell
npm test
```

Run one test file:

```powershell
npm test -- components/ProductDashboard.test.tsx
```

Run tests in watch mode:

```powershell
npm run test:watch
```

## Quality Checks

Run ESLint:

```powershell
npm run lint
```

Create a production build:

```powershell
npm run build
```
