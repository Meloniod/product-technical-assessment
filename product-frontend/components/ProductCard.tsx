"use client";

import type { Product } from "@/types/product";
import type { ProductSalesSummary } from "@/types/product";
import { SalesSummary } from "./SalesSummary";

interface ProductCardProps {
  product: Product;
  selected: boolean;
  summary: ProductSalesSummary | null;
  loading: boolean;
  error: string | null;
  onSelect: (product: Product) => void;
}

export function ProductCard({
  product,
  selected,
  summary,
  loading,
  error,
  onSelect,
}: ProductCardProps) {
  return (
    <article
      className={[
        "overflow-hidden rounded-lg border bg-white shadow-sm",
        "transition hover:-translate-y-1 hover:shadow-md",
        selected
          ? "border-blue-500 ring-2 ring-blue-100"
          : "border-gray-200",
      ].join(" ")}
    >
      <button
        type="button"
        onClick={() => onSelect(product)}
        aria-expanded={selected}
        className="group block w-full text-left focus:outline-none focus-visible:ring-2 focus-visible:ring-inset focus-visible:ring-blue-500"
      >
        <div className="aspect-square overflow-hidden bg-gray-100">
          <img
            src={product.image}
            alt={product.description}
            className="h-full w-full object-cover transition duration-300 group-hover:scale-105"
          />
        </div>

        <div className="space-y-2 p-4">
          <div className="flex items-start justify-between gap-3">
            <h2 className="font-semibold text-gray-900">
              {product.description}
            </h2>

            <span className="text-sm font-medium text-gray-600">
              {product.category}
            </span>
          </div>

          <p className="text-lg font-bold text-gray-900">
            {product.salePrice.toFixed(2)}
          </p>

          <p className="pt-1 text-sm font-medium text-blue-700">
            {selected ? "Hide sales summary" : "View sales summary"}
          </p>
        </div>
      </button>

      {selected && (
        <div className="border-t border-gray-100 px-4 py-4">
          <SalesSummary
            summary={summary}
            loading={loading}
            error={error}
          />
        </div>
      )}
    </article>
  );
}