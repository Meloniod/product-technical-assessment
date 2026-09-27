"use client";

import type { ProductSalesSummary } from "@/types/product";

interface SalesSummaryProps {
  productName: string;
  summary: ProductSalesSummary | null;
  loading: boolean;
  error: string | null;
}

export function SalesSummary({
  productName,
  summary,
  loading,
  error,
}: SalesSummaryProps) {
  if (loading) {
    return (
      <section className="rounded-xl border bg-white p-6 shadow-sm">
        <p className="text-gray-500">
          Loading sales summary for {productName}...
        </p>
      </section>
    );
  }

  if (error) {
    return (
      <section className="rounded-xl border border-red-200 bg-red-50 p-6">
        <h2 className="font-semibold text-red-900">
          Unable to load sales summary
        </h2>

        <p className="mt-2 text-sm text-red-700">
          {error}
        </p>
      </section>
    );
  }

  if (!summary) {
    return (
      <section className="rounded-xl border bg-white p-6 shadow-sm">
        <p className="text-gray-500">
          Select a product to view its sales summary.
        </p>
      </section>
    );
  }

  return (
    <section className="rounded-xl border bg-white p-6 shadow-sm">
      <div className="mb-6">
        <p className="text-sm text-gray-500">
          Sales summary
        </p>

        <h2 className="text-2xl font-bold text-gray-900">
          {productName}
        </h2>
      </div>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
        <SummaryItem
          label="Sales"
          value={summary.numberOfSales.toString()}
        />

        <SummaryItem
          label="Quantity"
          value={summary.totalQuantity.toLocaleString()}
        />

        <SummaryItem
          label="Total Sales"
          value={summary.totalSales.toFixed(2)}
        />
      </div>
    </section>
  );
}

interface SummaryItemProps {
  label: string;
  value: string;
}

function SummaryItem({
  label,
  value,
}: SummaryItemProps) {
  return (
    <div className="rounded-lg bg-gray-50 p-4">
      <p className="text-sm text-gray-500">
        {label}
      </p>

      <p className="mt-1 text-xl font-bold text-gray-900">
        {value}
      </p>
    </div>
  );
}