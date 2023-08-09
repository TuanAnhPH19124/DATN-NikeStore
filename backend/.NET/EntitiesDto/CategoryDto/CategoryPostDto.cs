using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.CategoryDto
{
    public class CategoryPostDto
    {
        [Required]
        public string Name { get; set; }
    }
}
