import { ShoppingCart } from 'lucide-react';

export default function ProductCard({ product, onAddToCart }) {
  const outOfStock = product.stock === 0;

  return (
    <div className="bg-white rounded-2xl shadow-sm hover:shadow-md transition-shadow overflow-hidden flex flex-col">
      <div className="aspect-[4/3] bg-orange-50 overflow-hidden">
        {product.imageUrl ? (
          <img
            src={product.imageUrl}
            alt={product.name}
            className="w-full h-full object-cover"
            onError={(e) => { e.target.style.display = 'none'; e.target.nextSibling.style.display = 'flex'; }}
          />
        ) : null}
        <div
          className={`w-full h-full bg-gradient-to-br from-orange-100 to-orange-200 flex items-center justify-center ${product.imageUrl ? 'hidden' : 'flex'}`}
          style={{ display: product.imageUrl ? 'none' : 'flex' }}
        >
          <span className="text-4xl font-bold text-orange-300">{product.name[0]}</span>
        </div>
      </div>

      <div className="p-4 flex flex-col flex-1">
        <div className="flex items-start justify-between gap-2 mb-1">
          <h3 className="font-semibold text-gray-900 text-sm leading-tight line-clamp-2">{product.name}</h3>
          <span className={`shrink-0 text-xs font-medium px-2 py-0.5 rounded-full ${
            outOfStock ? 'bg-gray-100 text-gray-500' : 'bg-green-100 text-green-700'
          }`}>
            {outOfStock ? 'Out of Stock' : 'In Stock'}
          </span>
        </div>

        {product.categoryName && (
          <span className="text-xs text-orange-600 font-medium mb-2">{product.categoryName}</span>
        )}

        <div className="mt-auto pt-3 flex items-center justify-between">
          <span className="text-lg font-bold text-orange-500">${product.price.toFixed(2)}</span>
          <button
            onClick={() => onAddToCart(product)}
            disabled={outOfStock}
            className="flex items-center gap-1.5 bg-orange-500 hover:bg-orange-600 disabled:bg-gray-200 disabled:cursor-not-allowed text-white disabled:text-gray-400 text-xs font-semibold px-3 py-1.5 rounded-lg transition"
          >
            <ShoppingCart size={14} />
            Add to Cart
          </button>
        </div>
      </div>
    </div>
  );
}
