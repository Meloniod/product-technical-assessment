import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";
import {
  getProducts,
  getProductSalesSummary
} from "./products";

describe("products API client", () => {
  beforeEach(() => {
    vi.stubEnv("NEXT_PUBLIC_API_BASE_URL", "http://localhost:5000");
  });

  afterEach(() => {
    vi.restoreAllMocks();
    vi.unstubAllEnvs();
  });

  it("gets all products", async () => {
    const products = [
      {
        id: 20,
        description: "Cherries",
        salePrice: 16.2,
        category: "Fruit",
        image: "https://example.com/cherries.jpg",
      },
    ];

    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        new Response(JSON.stringify(products), {
          status: 200,
          headers: {
            "Content-Type": "application/json",
          },
        })
      )
    );

    const result = await getProducts();

    expect(result).toEqual(products);

    expect(fetch).toHaveBeenCalledWith(
      expect.stringContaining("/api/products"),
      expect.objectContaining({
        method: "GET",
      })
    );
  });

  it("gets sales summary for only the selected product", async () => {
    const summary = {
      productId: 20,
      numberOfSales: 3,
      totalQuantity: 16400,
      totalSales: 1197.12,
    };

    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        new Response(JSON.stringify(summary), {
          status: 200,
          headers: {
            "Content-Type": "application/json",
          },
        })
      )
    );

    const result = await getProductSalesSummary(20);

    expect(result).toEqual(summary);

    expect(fetch).toHaveBeenCalledWith(
      expect.stringContaining(
        "/api/products/20/sales-summary"
      ),
      expect.objectContaining({
        method: "GET",
      })
    );
  });

  it("throws the API problem detail when the request fails", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue(
        new Response(
          JSON.stringify({
            title: "External service unavailable.",
            detail: "The product service could not be reached.",
          }),
          {
            status: 502,
            headers: {
              "Content-Type": "application/json",
            },
          }
        )
      )
    );

    await expect(getProducts()).rejects.toThrow(
      "The product service could not be reached."
    );
  });
});