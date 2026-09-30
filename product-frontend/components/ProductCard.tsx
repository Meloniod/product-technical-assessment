"use client";

import type { Product } from "@/types/product";
import Image from "next/image";

interface ProductCardProps {
  product: Product;
  selected: boolean;
  onSelect: (product: Product) => void;
}

export function ProductCard({
  product,
  selected,
  onSelect,
}: ProductCardProps) {
  return (
    <button
      type="button"
      onClick={() => onSelect(product)}
      aria-pressed={selected}
      aria-label={`View sales summary for ${product.description}`}
      className={[
        "group overflow-hidden rounded-xl border bg-white",
        "text-left shadow-sm transition",
        "hover:-translate-y-1 hover:shadow-md",
        "focus:outline-none focus:ring-2",
        "focus:ring-blue-500",
        selected
          ? "border-blue-500 ring-2 ring-blue-100"
          : "border-gray-200",
      ].join(" ")}
    >
      <div className="relative aspect-square overflow-hidden bg-gray-100">
        <Image
          src={product.image}
          alt={product.description}
          fill
          unoptimized
          sizes="(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 25vw"
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
      </div>
    </button>
  );
}