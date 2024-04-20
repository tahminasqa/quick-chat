using Quick.Chat.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Chat.Client.Pages
{
    public partial class GroupChat
    {
        public static Dictionary<string, string> GroupUsers
            = new Dictionary<string, string>();

        public static Dictionary<string, string> AllUsers = new Dictionary<string, string>();

        public async Task<Dictionary<string, string>> GetGroupUsersAsync()
        {
            //GroupUsers.Add("User1", "ahsan");
            //GroupUsers.Add("User2", "tahmina");
            //GroupUsers.Add("User3", "amrit");
            GroupUsers = await _chatManager.GetGroupUsersAsync();
            return GroupUsers;
        }
        public async Task<Dictionary<string, string>> GetAllUsersAsync()
        {
            //GroupUsers.Add("User1", "ahsan");
            //GroupUsers.Add("User2", "tahmina");
            //GroupUsers.Add("User3", "amrit");
            var contact = await _chatManager.GetAllUsersAsync();
            AllUsers = contact;
            return contact;
        }
    }
}
