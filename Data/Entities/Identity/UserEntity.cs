using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Identity
{
    public class UserEntity : IdentityUser<long>
    {
        [StringLength(255)]
        public string? ImageUrl { get; set; }
        public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
