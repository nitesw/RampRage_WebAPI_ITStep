using Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string? ImageUrl { get; set; }
        [StringLength(4000)]
        public string? Description { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual UserEntity? User { get; set; }
    }
}
