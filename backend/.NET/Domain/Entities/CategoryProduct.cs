using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CategoryProduct
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Category Category { get; set; }
    }
}


