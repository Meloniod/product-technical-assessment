export default function Loading() {
  return (
    <main className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-7xl px-6 py-10">
        <div className="animate-pulse space-y-6">
          <div className="h-10 w-48 rounded bg-gray-200" />

          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
            {Array.from({ length: 8 }).map(
              (_, index) => (
                <div
                  key={index}
                  className="overflow-hidden rounded-xl bg-white"
                >
                  <div className="aspect-square bg-gray-200" />

                  <div className="space-y-3 p-4">
                    <div className="h-5 w-2/3 rounded bg-gray-200" />
                    <div className="h-5 w-1/3 rounded bg-gray-200" />
                  </div>
                </div>
              )
            )}
          </div>
        </div>
      </div>
    </main>
  );
}