using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickwell.Models
{
    public class UserRecommendation
    {
        [Key]
        [ForeignKey("ApplicaitonUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int? reccomendation_1 { get; set; }
        public int? reccomendation_2 { get; set; }
        public int? reccomendation_3 { get; set; }
        public int? reccomendation_4 { get; set; }
        public int? reccomendation_5 { get; set; }
    }
}
