using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.CategoryDtos
{
    public class GetByIdCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
