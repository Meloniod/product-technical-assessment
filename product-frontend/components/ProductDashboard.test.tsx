import {
  afterEach,
  describe,
  expect,
  it,
  vi,
} from "vitest";
import {
  render,
  screen,
  waitFor,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";

import { ProductDashboard } from "./ProductDashboard";
import type { Product } from "@/types/product";

vi.mock("@/lib/api/products", () => {
  class ApiError extends Error {
    constructor(
      message: string,
      readonly status: number
    ) {
      super(message);
      this.name = "ApiError";
    }
  }

  return {
    ApiError,
    getProductSalesSummary: vi.fn(),
  };
});

import {
  ApiError,
  getProductSalesSummary
} from "@/lib/api/products";

const products: Product[] = [
  {
    id: 20,
    description: "Cherries",
    salePrice: 16.2,
    category: "Fruit",
    image: "https://example.com/cherries.jpg",
  },
  {
    id: 21,
    description: "Apples",
    salePrice: 12,
    category: "Fruit",
    image: "https://example.com/apples.jpg",
  },
];

describe("ProductDashboard", () => {
  afterEach(() => {
    vi.clearAllMocks();
  });

  it("does not request sales before a product is selected", () => {
    render(
      <ProductDashboard products={products} />
    );

    expect(
      getProductSalesSummary
    ).not.toHaveBeenCalled();
  });

  it("requests sales only for the selected product", async () => {
    const user = userEvent.setup();

    vi.mocked(getProductSalesSummary).mockResolvedValue({
      productId: 20,
      numberOfSales: 3,
      totalQuantity: 16400,
      totalSales: 1197.12,
    });

    render(
      <ProductDashboard products={products} />
    );

    await user.click(
      screen.getByRole("button", {
        name: /view sales summary for cherries/i
      })
    );

    await waitFor(() => {
      expect(
        getProductSalesSummary
      ).toHaveBeenCalledTimes(1);
    });

    expect(
      getProductSalesSummary
    ).toHaveBeenCalledWith(
      20,
      expect.any(AbortSignal)
    );

    expect(
      getProductSalesSummary
    ).not.toHaveBeenCalledWith(
      21,
      expect.anything()
    );
  });

  it("displays the returned sales summary", async () => {
    const user = userEvent.setup();

    vi.mocked(getProductSalesSummary).mockResolvedValue({
      productId: 20,
      numberOfSales: 3,
      totalQuantity: 16400,
      totalSales: 1197.12,
    });

    render(
      <ProductDashboard products={products} />
    );

    await user.click(
      screen.getByRole("button", {
        name: /view sales summary for cherries/i
      })
    );

    await waitFor(() => {
      expect(
        screen.getByText("16,400")
      ).toBeInTheDocument();
    });
  });

  it("displays an API error", async () => {
    const user = userEvent.setup();

    vi.mocked(
      getProductSalesSummary
    ).mockRejectedValue(
      new Error("Unable to load sales summary.")
    );

    render(
      <ProductDashboard products={products} />
    );

    await user.click(
      screen.getByRole("button", {
        name: /view sales summary for cherries/i
      })
    );

    await waitFor(() => {
      expect(
        screen.getByText(
          "Unable to load sales summary."
        )
      ).toBeInTheDocument();
    });
  });

  it("displays a not-found state for a 404 response", async () => {
    const user = userEvent.setup();

    vi.mocked(
      getProductSalesSummary
    ).mockRejectedValue(
      new ApiError("Product not found.", 404)
    );

    render(
      <ProductDashboard products={products} />
    );

    await user.click(
      screen.getByRole("button", {
        name: /view sales summary for cherries/i
      })
    );

    await waitFor(() => {
      expect(
        screen.getByRole("region", {
          name: /product not found/i,
        })
      ).toBeInTheDocument();
    });
  });
});