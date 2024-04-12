namespace Brickwell.Models
{
    public class Cart
    {
        public string ReturnUrl { get; set; } = "/";
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product prod, int quantity)
        {
            CartLine? line = Lines
                .Where(x => x.Product.product_ID == prod.product_ID)
                .FirstOrDefault();

            // If the product is not in the cart, add it.
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = prod,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        // Remove a product from the cart.
        public virtual void RemoveLine(Product prod) =>
            Lines.RemoveAll(x => x.Product.product_ID == prod.product_ID);

        // Clear the cart.
        public virtual void Clear() => Lines.Clear();

        // Compute the total sum of the cart.
        public decimal ComputeTotalSum() =>
            Lines.Sum(e => e.Product.price * e.Quantity);

        // The class that represents a line in the cart.
        public class CartLine 
        { 
            public int CartItemId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
