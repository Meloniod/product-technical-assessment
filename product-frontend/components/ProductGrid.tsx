"use client";

import type { Product } from "@/types/product";
import { ProductCard } from "./ProductCard";

interface ProductGridProps {
  products: Product[];
  selectedProductId: number | null;
  onSelect: (product: Product) => void;
}

export function ProductGrid({
  products,
  selectedProductId,
  onSelect,
}: ProductGridProps) {
  if (products.length === 0) {
    return (
      <p className="py-12 text-center text-gray-500">
        No products available.
      </p>
    );
  }

  return (
    <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      {products.map((product) => (
        <ProductCard
          key={product.id}
          product={product}
          selected={
            product.id === selectedProductId
          }
          onSelect={onSelect}
        />
      ))}
    </div>
  );
}