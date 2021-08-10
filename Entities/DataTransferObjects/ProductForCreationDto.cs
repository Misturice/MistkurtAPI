using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ProductForCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tag is required")]
        public string Tag { get; set; }
        
        public string Type { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        public float Cost { get; set; }
    }
}
