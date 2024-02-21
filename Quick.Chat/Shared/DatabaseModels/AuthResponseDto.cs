using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Chat.Shared.DatabaseModels
{
    public record AuthResponseDto(UserDto User, string Token);
}
