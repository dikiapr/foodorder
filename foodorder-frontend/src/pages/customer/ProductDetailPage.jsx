import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, ShoppingCart, Minus, Plus } from 'lucide-react';
import toast from 'react-hot-toast';
import { getProduct } from '../../api/productApi';
import { addToCart } from '../../api/cartApi';
import useCartStore from '../../store/cartStore';

export default function ProductDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const fetchCount = useCartStore((s) => s.fetchCount);

  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [quantity, setQuantity] = useState(1);
  const [adding, setAdding] = useState(false);

  useEffect(() => {
    setLoading(true);
    getProduct(id)
      .then((r) => setProduct(r.data))
      .catch(() => { toast.error('Product not found'); navigate('/home'); })
      .finally(() => setLoading(false));
  }, [id]);

  const handleAddToCart = async () => {
    setAdding(true);
    try {
      await addToCart({ productId: product.id, quantity });
      await fetchCount();
      toast.success(`${product.name} added to cart`);
    } catch (err) {
      toast.error(err.response?.data?.message || 'Failed to add to cart');
    } finally {
      setAdding(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="w-8 h-8 border-4 border-orange-400 border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  if (!product) return null;

  const outOfStock = product.stock === 0;

  return (
    <div>
      <button
        onClick={() => navigate(-1)}
        className="flex items-center gap-2 text-sm text-gray-500 hover:text-orange-500 transition mb-6"
      >
        <ArrowLeft size={16} />
        Back to Menu
      </button>

      <div className="bg-white rounded-2xl shadow-sm overflow-hidden">
        <div className="flex flex-col md:flex-row">
          {/* Image */}
          <div className="md:w-1/2 aspect-video md:aspect-auto bg-orange-50 flex items-center justify-center overflow-hidden">
            {product.imageUrl ? (
              <img src={product.imageUrl} alt={product.name} className="w-full h-full object-cover" />
            ) : (
              <div className="w-full h-full min-h-72 bg-gradient-to-br from-orange-100 to-orange-200 flex items-center justify-center">
                <span className="text-8xl font-bold text-orange-300">{product.name[0]}</span>
              </div>
            )}
          </div>

          {/* Info */}
          <div className="md:w-1/2 p-8 flex flex-col">
            <div className="flex items-start gap-3 mb-2">
              {product.categoryName && (
                <span className="text-xs font-semibold px-3 py-1 rounded-full bg-orange-100 text-orange-600">
                  {product.categoryName}
                </span>
              )}
              <span className={`text-xs font-semibold px-3 py-1 rounded-full ${
                outOfStock ? 'bg-gray-100 text-gray-500' : 'bg-green-100 text-green-600'
              }`}>
                {outOfStock ? 'Out of Stock' : 'In Stock'}
              </span>
            </div>

            <h1 className="text-2xl font-bold text-gray-900 mb-2">{product.name}</h1>

            {product.description && (
              <p className="text-sm text-gray-500 leading-relaxed mb-4">{product.description}</p>
            )}

            <div className="text-3xl font-bold text-orange-500 mb-1">
              ${product.price.toFixed(2)}
            </div>
            <p className="text-xs text-gray-400 mb-6">{product.stock} left in stock</p>

            {/* Quantity */}
            <div className="flex items-center gap-4 mb-6">
              <span className="text-sm font-medium text-gray-700">Quantity</span>
              <div className="flex items-center gap-3 bg-gray-50 rounded-xl px-3 py-2">
                <button
                  onClick={() => setQuantity((q) => Math.max(1, q - 1))}
                  className="w-6 h-6 flex items-center justify-center rounded-full hover:bg-gray-200 transition"
                >
                  <Minus size={14} />
                </button>
                <span className="w-6 text-center text-sm font-semibold">{quantity}</span>
                <button
                  onClick={() => setQuantity((q) => Math.min(product.stock, q + 1))}
                  disabled={outOfStock}
                  className="w-6 h-6 flex items-center justify-center rounded-full hover:bg-gray-200 disabled:opacity-40 transition"
                >
                  <Plus size={14} />
                </button>
              </div>
            </div>

            <button
              onClick={handleAddToCart}
              disabled={outOfStock || adding}
              className="flex items-center justify-center gap-2 bg-orange-500 hover:bg-orange-600 disabled:bg-gray-200 disabled:cursor-not-allowed text-white disabled:text-gray-400 font-semibold py-3 px-6 rounded-xl transition text-sm"
            >
              <ShoppingCart size={18} />
              {adding ? 'Adding...' : `Add to Cart — $${(product.price * quantity).toFixed(2)}`}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
