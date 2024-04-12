using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickwell.Models
{
    public class CustomerRecommendation
    {
        [Key]
        [ForeignKey("Customer")]
        public int customer_ID { get; set; }
        public Customer Customer { get; set; }
        public int? reccomendation_1 { get; set; }
        public int? reccomendation_2 { get; set; }
        public int? reccomendation_3 { get; set; }
        public int? reccomendation_4 { get; set; }
        public int? reccomendation_5 { get; set; }
    }
}
