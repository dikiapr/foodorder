const config = {
  Pending:   { bg: 'bg-amber-100',  text: 'text-amber-700',  label: 'Pending' },
  Paid:      { bg: 'bg-blue-100',   text: 'text-blue-700',   label: 'Paid' },
  Completed: { bg: 'bg-green-100',  text: 'text-green-700',  label: 'Completed' },
  Cancelled: { bg: 'bg-red-100',    text: 'text-red-700',    label: 'Cancelled' },
};

export default function StatusBadge({ status }) {
  const c = config[status] ?? { bg: 'bg-gray-100', text: 'text-gray-600', label: status };
  return (
    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-semibold ${c.bg} ${c.text}`}>
      {c.label}
    </span>
  );
}
