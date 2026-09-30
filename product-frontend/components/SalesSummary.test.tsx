import { describe, expect, it } from "vitest";
import { render, screen } from "@testing-library/react";
import { SalesSummary } from "./SalesSummary";

describe("SalesSummary", () => {
  it("shows the initial state when no product is selected", () => {
    render(
      <SalesSummary
        productName=""
        summary={null}
        loading={false}
        error={null}
      />
    );

    expect(
      screen.getByText(/select a product/i)
    ).toBeInTheDocument();
  });

  it("shows loading state", () => {
    render(
      <SalesSummary
        productName="Cherries"
        summary={null}
        loading={true}
        error={null}
      />
    );

    expect(
      screen.getByText(/loading sales summary/i)
    ).toBeInTheDocument();
  });

  it("shows an error", () => {
    render(
      <SalesSummary
        productName="Cherries"
        summary={null}
        loading={false}
        error="Unable to load sales summary."
      />
    );

    expect(
      screen.getByText("Unable to load sales summary.")
    ).toBeInTheDocument();
  });

  it("renders the sales summary", () => {
    render(
      <SalesSummary
        productName="Cherries"
        loading={false}
        error={null}
        summary={{
          productId: 20,
          numberOfSales: 3,
          totalQuantity: 16400,
          totalSales: 1197.12,
        }}
      />
    );

    expect(
      screen.getByText("Cherries")
    ).toBeInTheDocument();

    expect(
      screen.getByText("3")
    ).toBeInTheDocument();

    expect(
      screen.getByText("16,400")
    ).toBeInTheDocument();

    expect(
      screen.getByText(/1,197\.12/)
    ).toBeInTheDocument();
  });
});