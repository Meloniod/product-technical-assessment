"use client";

interface ErrorPageProps {
  error: Error & {
    digest?: string;
  };
  reset: () => void;
}

export default function ErrorPage({
  error,
  reset,
}: ErrorPageProps) {
  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-50 px-6">
      <div className="max-w-md text-center">
        <h1 className="text-2xl font-bold text-gray-900">
          Unable to load products
        </h1>

        <p className="mt-3 text-gray-600">
          {error.message}
        </p>

        <button
          type="button"
          onClick={() => reset()}
          className="mt-6 rounded-lg bg-gray-900 px-5 py-3 font-medium text-white hover:bg-gray-800"
        >
          Try again
        </button>
      </div>
    </main>
  );
}