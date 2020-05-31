using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Ef.Entities
{
    public class WebsiteField : BaseEntity
    { 
        [ForeignKey(nameof(Website))]
        public int WebsiteId { get; set; }
        public Website Website { get; set; }

        [Required]
        public string FieldName { get; set; }
         
        public string FieldValue { get; set; }
    }
}
