using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public string ParentCategoriesId { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual List<Category> ChildCategories { get; set; }
        public virtual List<CategoryProduct> CategoryProducts { get; set; }
    }
}
