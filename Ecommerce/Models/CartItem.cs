namespace Ecommerce.Models
{
    public class CartItem
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal SubTotal => Price * Quantity;

        public CartItem()
        { } // Konstruktor default diperlukan untuk beberapa skenario

        public CartItem(string productId, string productName, decimal price, int quantity, string imageUrl)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            ImageUrl = imageUrl;
        }
    }
}