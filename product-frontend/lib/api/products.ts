import type {
  Product
} from "@/types/product";
import { ProductSalesSummary } from "@/types/productSalesSummary";

function getApiBaseUrl(): string {
  const baseUrl =
    process.env.NEXT_PUBLIC_API_BASE_URL;

  if (!baseUrl) {
    throw new Error(
      "NEXT_PUBLIC_API_BASE_URL is not configured."
    );
  }

  return baseUrl.replace(/\/$/, "");
}

async function handleResponse<T>(
  response: Response
): Promise<T> {
  if (!response.ok) {
    let message =
      "An unexpected error occurred.";

    try {
      const problem =
        await response.json();

      if (problem.detail) {
        message = problem.detail;
      } else if (problem.title) {
        message = problem.title;
      }
    } catch {
      // Keep the default message.
    }

    throw new Error(message);
  }

  return response.json() as Promise<T>;
}

export async function getProducts(
  signal?: AbortSignal
): Promise<Product[]> {
  const response = await fetch(
    `${getApiBaseUrl()}/api/products`,
    {
      method: "GET",
      signal,
    }
  );

  return handleResponse<Product[]>(
    response
  );
}

export async function getProductSalesSummary(
  productId: number,
  signal?: AbortSignal
): Promise<ProductSalesSummary> {
  const response = await fetch(
    `${getApiBaseUrl()}/api/products/${productId}/sales-summary`,
    {
      method: "GET",
      signal,
    }
  );

  return handleResponse<ProductSalesSummary>(
    response
  );
}