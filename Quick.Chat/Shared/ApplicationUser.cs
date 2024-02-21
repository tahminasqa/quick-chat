using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Quick.Chat.Shared
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }
        public ApplicationUser()
        {
            ChatMessagesFromUsers = new HashSet<ChatMessage>();
            ChatMessagesToUsers = new HashSet<ChatMessage>();
        }
    }
}
