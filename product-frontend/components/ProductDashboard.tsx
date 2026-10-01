"use client";

import { useEffect, useRef, useState } from "react";

import type {
  Product
} from "@/types/product";

import {
  ApiError,
  getProductSalesSummary
} from "@/lib/api/products";

import { ProductGrid } from "./ProductGrid";
import { SalesSummary } from "./SalesSummary";
import { ProductSalesSummary } from "@/types/productSalesSummary";

interface ProductDashboardProps {
  products: Product[];
}

export function ProductDashboard({
  products,
}: ProductDashboardProps) {
  const [selectedProduct, setSelectedProduct] =
    useState<Product | null>(null);

  const [summary, setSummary] =
    useState<ProductSalesSummary | null>(null);

  const [loading, setLoading] =
    useState(false);

  const [error, setError] =
    useState<string | null>(null);

  const [notFound, setNotFound] =
    useState(false);

  const requestController =
    useRef<AbortController | null>(null);

  useEffect(() => {
    return () => {
      requestController.current?.abort();
    };
  }, []);

  async function handleProductSelect(
    product: Product
  ) {
    requestController.current?.abort();

    const controller =
      new AbortController();

    requestController.current = controller;

    setSelectedProduct(product);
    setSummary(null);
    setError(null);
    setNotFound(false);
    setLoading(true);

    try {
      const result =
        await getProductSalesSummary(
          product.id,
          controller.signal
        );

      if (!controller.signal.aborted) {
        setSummary(result);
      }
    } catch (error) {
      if (
        error instanceof DOMException &&
        error.name === "AbortError"
      ) {
        return;
      }

      if (!controller.signal.aborted) {
        if (error instanceof ApiError && error.status === 404) {
          setNotFound(true);
        } else {
          setError(
            error instanceof Error
              ? error.message
              : "Unable to load sales summary."
          );
        }
      }
    } finally {
      if (!controller.signal.aborted) {
        setLoading(false);
      }
    }
  }

  return (
    <div className="space-y-10">
      <ProductGrid
        products={products}
        selectedProductId={
          selectedProduct?.id ?? null
        }
        onSelect={handleProductSelect}
      />

      <SalesSummary
        productName={selectedProduct?.description ?? ""}
        summary={summary}
        loading={loading}
        error={error}
        notFound={notFound}
      />
    </div>
  );
}