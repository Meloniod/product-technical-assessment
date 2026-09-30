import { ProductDashboard } from "@/components/ProductDashboard";
import { getProducts } from "@/lib/api/products";

export default async function HomePage() {
  const products = await getProducts();
  return (
    <main className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-7xl px-6 py-10">
        <header className="mb-10">
          <p className="text-sm font-medium text-gray-500">
            Product Sales
          </p>

          <h1 className="mt-2 text-4xl font-bold tracking-tight text-gray-900">
            Products
          </h1>

          <p className="mt-3 max-w-2xl text-gray-600">
            Select a product to view its sales summary.
          </p>
        </header>

        <ProductDashboard
          products={products}
        />
      </div>
    </main>
  );
}