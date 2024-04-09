using System.ComponentModel.DataAnnotations.Schema;

namespace Brickwell.Models
{
    public class LineItem
    {
        [ForeignKey("Order")]
        public required int transaction_ID { get; set; }
        public required Order Order { get; set; }
        [ForeignKey("Product")]
        public required int product_ID { get; set; }
        public required Product Product { get; set; }
        public required int qty { get; set; }
        public int? rating { get; set; }
    }
}
