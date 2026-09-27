"use client";

import type { ProductSalesSummary } from "@/types/product";

interface SalesSummaryProps {
  summary: ProductSalesSummary | null;
  loading: boolean;
  error: string | null;
}

export function SalesSummary({
  summary,
  loading,
  error,
}: SalesSummaryProps) {
  if (loading) {
    return (
      <p className="text-sm text-gray-500" role="status">
        Loading sales summary...
      </p>
    );
  }

  if (error) {
    return (
      <section aria-label="Sales summary error">
        <h3 className="text-sm font-semibold text-red-900">
          Unable to load sales summary
        </h3>

        <p className="mt-1 text-sm text-red-700">
          {error}
        </p>
      </section>
    );
  }

  if (!summary) {
    return null;
  }

  return (
    <section aria-label="Sales summary">
      <h3 className="text-sm font-semibold text-gray-700">
        Sales summary
      </h3>

      <dl className="mt-3 space-y-2">
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
      </dl>
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
    <div className="flex items-baseline justify-between gap-3 border-b border-gray-100 pb-2 last:border-0 last:pb-0">
      <dt className="text-sm text-gray-500">
        {label}
      </dt>

      <dd className="text-right text-sm font-semibold tabular-nums text-gray-900">
        {value}
      </dd>
    </div>
  );
}