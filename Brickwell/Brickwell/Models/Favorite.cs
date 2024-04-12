using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickwell.Models
{
    public class Favorite
    {
        [Key]
        public string type { get; set; }
        public int? favorite_1 { get; set; }
        public int? favorite_2 { get; set; }
        public int? favorite_3 { get; set; }
        public int? favorite_4 { get; set; }
        public int? favorite_5 { get; set; }
    }
}
