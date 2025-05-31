using Ecommerce.Models;

namespace Ecommerce.Services
{
    public class CartService
    {
        private List<CartItem> _items = new List<CartItem>();

        // Event yang akan dipicu ketika isi keranjang berubah
        public event Action? OnCartChanged;

        public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

        public void AddItem(CartItem newItem)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == newItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                _items.Add(newItem);
            }
            NotifyCartChanged();
        }

        public void RemoveItem(string productId)
        {
            var itemToRemove = _items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove != null)
            {
                _items.Remove(itemToRemove);
                NotifyCartChanged();
            }
        }

        public void UpdateQuantity(string productId, int newQuantity)
        {
            var itemToUpdate = _items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToUpdate != null)
            {
                if (newQuantity > 0)
                {
                    itemToUpdate.Quantity = newQuantity;
                }
                else
                {
                    _items.Remove(itemToUpdate); // Hapus jika kuantitas 0 atau kurang
                }
                NotifyCartChanged();
            }
        }

        public decimal GetTotal()
        {
            return _items.Sum(item => item.SubTotal);
        }

        public int GetTotalItems()
        {
            return _items.Sum(item => item.Quantity);
        }

        public void ClearCart()
        {
            _items.Clear();
            NotifyCartChanged();
        }

        private void NotifyCartChanged()
        {
            OnCartChanged?.Invoke();
        }
    }
}