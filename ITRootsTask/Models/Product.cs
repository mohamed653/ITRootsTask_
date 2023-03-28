using System;

namespace ITRootsTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

    }
}
