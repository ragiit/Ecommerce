using Ecommerce.Models;
using static Ecommerce.Components.Pages.Products;

namespace Ecommerce.Services
{
    public class ProductAdminService
    {
        // Menggunakan List statis agar data bertahan antar permintaan (untuk demo in-memory)
        // Dalam aplikasi nyata, ini akan terhubung ke database.
        private static List<ProductItem> _products = InitializeProductsInternal();

        public event Action? OnProductsChanged;

        private static List<ProductItem> InitializeProductsInternal()
        {
            // Data awal yang sama seperti di halaman Produk.razor sebelumnya
            return new List<ProductItem>
        {
            new ProductItem {
                Id = "kaos-custom-sablon-dtf", // Gunakan ID yang lebih deskriptif jika memungkinkan
                Name = "Kaos Custom Sablon DTF",
                ShortDescription = "Kaos katun berkualitas dengan sablon DTF full color. Desain suka-suka!",
                FullDescription = "Detail lengkap kaos custom sablon DTF...",
                Price = 85000,
                Category = "Apparel",
                Tag = "Best Seller",
                ImageUrl = "https://placehold.co/400x300/FBCFE8/831843?text=Kaos+DTF",
                Stock = 50,
                IsFeatured = true
            },
            new ProductItem {
                Id = "mug-keramik-custom-foto",
                Name = "Mug Keramik Custom Foto",
                ShortDescription = "Cetak foto, logo, atau tulisan di mug keramik. Cocok untuk souvenir.",
                FullDescription = "Detail lengkap mug keramik custom...",
                Price = 45000,
                Category = "Souvenir",
                ImageUrl = "https://placehold.co/400x300/A5B4FC/312E81?text=Mug+Custom",
                Stock = 100
            },
            new ProductItem {
                Id = "payung-lipat-promosi",
                Name = "Payung Lipat Promosi",
                ShortDescription = "Payung lipat otomatis dengan sablon logo perusahaan Anda.",
                FullDescription = "Detail lengkap payung lipat promosi...",
                Price = 0, // "Hubungi Kami"
                Category = "Merchandise",
                Tag = "Promosi",
                ImageUrl = "https://placehold.co/400x300/93C5FD/1E40AF?text=Payung+Promosi",
                Stock = 200
            },
            // Tambahkan produk lain jika perlu
        };
        }

        public List<ProductItem> GetProducts()
        {
            return _products.OrderBy(p => p.Name).ToList();
        }

        public ProductItem? GetProductById(string id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(ProductItem product)
        {
            if (string.IsNullOrWhiteSpace(product.Id) || _products.Any(p => p.Id == product.Id))
            {
                product.Id = Guid.NewGuid().ToString("N"); // Pastikan ID unik jika kosong atau duplikat
            }
            _products.Add(product);
            NotifyProductsChanged();
        }

        public void UpdateProduct(ProductItem product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.ShortDescription = product.ShortDescription;
                existingProduct.FullDescription = product.FullDescription;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Category = product.Category;
                existingProduct.Tag = product.Tag;
                existingProduct.WhatsAppNumber = product.WhatsAppNumber;
                existingProduct.Stock = product.Stock;
                existingProduct.IsFeatured = product.IsFeatured;
                NotifyProductsChanged();
            }
        }

        public void DeleteProduct(string id)
        {
            var productToRemove = _products.FirstOrDefault(p => p.Id == id);
            if (productToRemove != null)
            {
                _products.Remove(productToRemove);
                NotifyProductsChanged();
            }
        }

        public List<string> GetCategories()
        {
            return _products.Select(p => p.Category).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().OrderBy(c => c).ToList();
        }

        private void NotifyProductsChanged()
        {
            OnProductsChanged?.Invoke();
        }
    }
}