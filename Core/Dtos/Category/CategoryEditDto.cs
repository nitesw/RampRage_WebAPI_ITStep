using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Category
{
    public class CategoryEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
    }
}
