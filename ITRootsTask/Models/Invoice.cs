using System.ComponentModel.DataAnnotations;

namespace ITRootsTask.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        [Display(Name="Invoice Name")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products  { get; set; }
    }
}
