using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Seeder
{
    public class SeederUserModel
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
