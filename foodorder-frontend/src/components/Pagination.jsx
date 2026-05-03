export default function Pagination({ page, totalPages, onChange }) {
  if (totalPages <= 1) return null;

  const pages = [];
  for (let i = 1; i <= totalPages; i++) pages.push(i);

  return (
    <div className="flex items-center justify-center gap-1 mt-6">
      <button
        onClick={() => onChange(page - 1)}
        disabled={page === 1}
        className="px-3 py-1.5 rounded-lg text-sm font-medium text-gray-600 hover:bg-orange-50 disabled:opacity-40 disabled:cursor-not-allowed transition"
      >
        ‹
      </button>
      {pages.map((p) => (
        <button
          key={p}
          onClick={() => onChange(p)}
          className={`px-3 py-1.5 rounded-lg text-sm font-semibold transition ${
            p === page
              ? 'bg-orange-500 text-white'
              : 'text-gray-600 hover:bg-orange-50'
          }`}
        >
          {p}
        </button>
      ))}
      <button
        onClick={() => onChange(page + 1)}
        disabled={page === totalPages}
        className="px-3 py-1.5 rounded-lg text-sm font-medium text-gray-600 hover:bg-orange-50 disabled:opacity-40 disabled:cursor-not-allowed transition"
      >
        ›
      </button>
    </div>
  );
}
