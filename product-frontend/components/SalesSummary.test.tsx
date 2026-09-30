import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { SalesSummary } from "./SalesSummary";

describe("SalesSummary", () => {
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
      screen.getByRole("status")
    ).toHaveTextContent("Loading sales summary...");
  });

  it("shows an error", () => {
    render(
      <SalesSummary
        productName="Cherries"
        summary={null}
        loading={false}
        error="Unable to contact the sales service."
      />
    );

    expect(
      screen.getByRole("region", {
        name: /sales summary error/i,
      })
    ).toBeInTheDocument();

    expect(
      screen.getByText("Unable to contact the sales service.")
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
      screen.getByRole("region", {
        name: /sales summary/i,
      })
    ).toBeInTheDocument();

    expect(
      screen.getByRole("heading", {
        name: /sales summary for cherries/i,
      })
    ).toBeInTheDocument();

    expect(screen.getByText("Sales")).toBeInTheDocument();
    expect(screen.getByText("3")).toBeInTheDocument();

    expect(screen.getByText("Quantity")).toBeInTheDocument();
    expect(screen.getByText("16,400")).toBeInTheDocument();

    expect(screen.getByText("Total Sales")).toBeInTheDocument();
    expect(screen.getByText("1197.12")).toBeInTheDocument();
  });

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
      screen.queryByRole("region", {
        name: /sales summary/i,
      })
    ).not.toBeInTheDocument();
  });
});