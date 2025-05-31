namespace Ecommerce.Models
{
    public class ProductItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty; // Tambahkan untuk detail
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "https://placehold.co/400x300/E2E8F0/A0AEC0?text=Produk";
        public string Category { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string WhatsAppNumber { get; set; } = "6285156615269"; // Nomor WA default
        public int Stock { get; set; } = 10; // Contoh: Tambahkan stok
        public bool IsFeatured { get; set; } = false; // Contoh: Untuk produk unggulan di beranda
    }
}