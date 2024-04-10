using System.ComponentModel.DataAnnotations;

namespace Brickwell.Models
{
    public class Customer
    {
        [Key]
        public required int customer_ID { get; set; }

        // This is a foreign key tht hasn't been implimeneted to connect with our login/ authorization system
        public string? username { get; set; }
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required string birth_date { get; set; }
        public required string country_of_residence { get; set; }
        public required string gender { get; set; }
        public required int age { get; set; }
    }
}
