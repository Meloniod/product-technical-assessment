"use client";

import { useEffect, useRef, useState } from "react";

import { ProductGrid } from "@/components/ProductGrid";
import { SalesSummary } from "@/components/SalesSummary";

import {
  getProductSalesSummary,
  getProducts,
} from "@/lib/api/products";

import type {
  Product,
  ProductSalesSummary,
} from "@/types/product";

export default function Home() {
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedProduct, setSelectedProduct] =
    useState<Product | null>(null);

  const [salesSummary, setSalesSummary] =
    useState<ProductSalesSummary | null>(null);

  const [loadingProducts, setLoadingProducts] =
    useState(true);

  const [loadingSales, setLoadingSales] =
    useState(false);

  const [error, setError] =
    useState<string | null>(null);

  const [salesError, setSalesError] =
    useState<string | null>(null);

  const salesAbortController =
    useRef<AbortController | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    async function loadProducts() {
      try {
        setLoadingProducts(true);
        setError(null);

        const result = await getProducts(
          controller.signal
        );

        setProducts(result);
      } catch (error) {
        if (
          error instanceof DOMException &&
          error.name === "AbortError"
        ) {
          return;
        }

        setError(
          error instanceof Error
            ? error.message
            : "Unable to load products."
        );
      } finally {
        setLoadingProducts(false);
      }
    }

    loadProducts();

    return () => {
      controller.abort();
    };
  }, []);

  async function handleProductSelect(
    product: Product
  ) {
    salesAbortController.current?.abort();

    const controller =
      new AbortController();

    salesAbortController.current =
      controller;

    setSelectedProduct(product);
    setSalesSummary(null);
    setSalesError(null);
    setLoadingSales(true);

    try {
      const summary =
        await getProductSalesSummary(
          product.id,
          controller.signal
        );

      if (!controller.signal.aborted) {
        setSalesSummary(summary);
      }
    } catch (error) {
      if (
        error instanceof DOMException &&
        error.name === "AbortError"
      ) {
        return;
      }

      setSalesError(
        error instanceof Error
          ? error.message
          : "Unable to load sales summary."
      );
    } finally {
      if (!controller.signal.aborted) {
        setLoadingSales(false);
      }
    }
  }

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-7xl px-6 py-10">
        <header className="mb-10">
          <p className="text-sm font-medium text-blue-600">
            Product Dashboard
          </p>

          <h1 className="mt-2 text-3xl font-bold text-gray-900">
            Products
          </h1>

          <p className="mt-2 max-w-2xl text-gray-600">
            Select a product to view its sales summary.
          </p>
        </header>

        {loadingProducts && (
          <p className="py-12 text-center text-gray-500">
            Loading products...
          </p>
        )}

        {error && (
          <div className="mb-8 rounded-xl border border-red-200 bg-red-50 p-6">
            <h2 className="font-semibold text-red-900">
              Unable to load products
            </h2>

            <p className="mt-2 text-sm text-red-700">
              {error}
            </p>
          </div>
        )}

        {!loadingProducts && !error && (
          <ProductGrid
            products={products}
            selectedProductId={
              selectedProduct?.id ?? null
            }
            onSelect={handleProductSelect}
          />
        )}

        <div className="mt-10">
          <SalesSummary
            productName={
              selectedProduct?.description ?? ""
            }
            summary={salesSummary}
            loading={loadingSales}
            error={salesError}
          />
        </div>
      </div>
    </main>
  );
}