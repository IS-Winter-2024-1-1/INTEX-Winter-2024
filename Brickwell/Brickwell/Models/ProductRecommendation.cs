using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickwell.Models
{
    public class ProductRecommendation
    {
        [Key]
        [ForeignKey("Product")]
        public int product_ID { get; set; }
        public Product Product { get; set; }
        public int? recommendation_1 { get; set; }
        public int? recommendation_2 { get; set; }
        public int? recommendation_3 { get; set; }
        public int? recommendation_4 { get; set; }
        public int? recommendation_5 { get; set; }
    }
}
