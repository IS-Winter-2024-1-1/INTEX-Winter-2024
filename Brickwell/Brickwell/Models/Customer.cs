using System.ComponentModel.DataAnnotations;

namespace Brickwell.Models
{
    public class Customer
    {
        [Key]
        public required int customer_ID { get; set; }
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required DateOnly birth_date { get; set; }
        public required string country_of_residence { get; set; }
        public required string gender { get; set; }
        public required int age { get; set; }
    }
}
