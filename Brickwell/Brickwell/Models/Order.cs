using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickwell.Models
{
    public class Order
    {
        [Key]
        public required int transaction_ID { get; set; }
        [ForeignKey("Customer")]
        public required int customer_ID { get; set; }
        public required Customer Customer { get; set; }
        public required DateOnly date { get; set; }
        public required string day_of_week { get; set; }
        public required int time { get; set; }
        public required string entry_mode { get; set; }
        public required int amount { get; set; }
        public required string type_of_transaction { get; set; }
        public required string country_of_transaction { get; set; }
        public required string shipping_address { get; set; }
        public required string bank { get; set; }
        public required string type_of_card { get; set; }
        public int? fraud { get; set; }
    }
}
