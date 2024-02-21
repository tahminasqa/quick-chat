using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Chat.Shared.DatabaseModels
{
    public class RegisterDto
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(20), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
