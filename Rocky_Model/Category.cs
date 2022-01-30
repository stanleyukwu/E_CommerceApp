using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky_Utility.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,int.MaxValue, ErrorMessage = "Display order must be greater than 0")]
        [Required]
        public int DisplayOrder { get; set; }
    }
}
