import { describe, expect, it, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { ProductCard } from "./ProductCard";
import type { Product } from "@/types/product";

const product: Product = {
  id: 20,
  description: "Cherries",
  salePrice: 16.2,
  category: "Fruit",
  image: "https://example.com/cherries.jpg",
};

describe("ProductCard", () => {
  it("renders product information", () => {
    render(
      <ProductCard
        product={product}
        selected={false}
        onSelect={vi.fn()}
      />
    );

    expect(
      screen.getByText("Cherries")
    ).toBeInTheDocument();

    expect(
      screen.getByText("Fruit")
    ).toBeInTheDocument();

    expect(
      screen.getByText(/16\.20/)
    ).toBeInTheDocument();
  });

  it("calls onSelect when clicked", async () => {
    const user = userEvent.setup();
    const onSelect = vi.fn();

    render(
      <ProductCard
        product={product}
        selected={false}
        onSelect={onSelect}
      />
    );

    await user.click(
      screen.getByRole("button", {
        name: /view sales summary for cherries/i
      })
    );

    expect(onSelect).toHaveBeenCalledWith(product);
  });

  it("supports keyboard selection", async () => {
    const user = userEvent.setup();
    const onSelect = vi.fn();

    render(
      <ProductCard
        product={product}
        selected={false}
        onSelect={onSelect}
      />
    );

    const button = screen.getByRole("button", {
      name: /view sales summary for cherries/i
    });

    button.focus();

    await user.keyboard("{Enter}");

    expect(onSelect).toHaveBeenCalledWith(product);
  });

  it("sets aria-pressed when selected", () => {
    render(
      <ProductCard
        product={product}
        selected={true}
        onSelect={vi.fn()}
      />
    );

    expect(
      screen.getByRole("button", {
        name: /view sales summary for cherries/i
      })
    ).toHaveAttribute("aria-pressed", "true");
  });
});