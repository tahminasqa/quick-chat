using Quick.Chat.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Quick.Chat.Client.Managers
{
    public class ChatManager : IChatManager
    {
        private readonly HttpClient _httpClient;

        public ChatManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ChatMessage>> GetConversationAsync(string contactId)
        {
            return await _httpClient.GetFromJsonAsync<List<ChatMessage>>($"api/chat/{contactId}");
        }
        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<ApplicationUser>($"api/chat/users/{userId}");
        }
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            var data = await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/chat/users");
            return data;
        }
        public async Task<Dictionary<string, string>> GetGroupUsersAsync()
        {
            var data = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>("api/chat/groupusers");
            return data;
        }
        public async Task<Dictionary<string,string>> GetAllUsersAsync()
        {
            var data = await _httpClient.GetFromJsonAsync<Dictionary<string,string>>("api/chat/allusers");
            return data;
        }
        public async Task SaveMessageAsync(ChatMessage message)
        {
            await _httpClient.PostAsJsonAsync("api/chat", message);
        }

        public async Task<SystemStats> NumOfRegisteredUserAsync()
        {
            int numOfUser = await _httpClient.GetFromJsonAsync<int>("api/serverstats/registeredusers");
            var data = new List<SystemData>
            {
                new SystemData
                {
                   NumOfUser = numOfUser,
                   Category = "Registered User"
                 },new SystemData
                {
                   NumOfUser = 2,
                   Category = "Online User"
                 }
            };

            return new SystemStats
            {
                Data = data,
                Label = "Users Stats"
            };

        }

        public async Task<SystemStats> NumOfMessagesAsync()
        {
            int numOfMessage = await _httpClient.GetFromJsonAsync<int>("api/serverstats/numberOfMessage");


            var data = new List<SystemData>
            {
                new SystemData
                {
                   NumOfUser = numOfMessage,
                   Category = "Num Of Message"
                },
                new SystemData
                {
                   NumOfUser = 3,
                   Category = "Num Of User"
                }
            };

            return new SystemStats
            {
                Data = data,
                Label = "One to One Message Stats"
            };
        }
        public async Task<SystemStats> NumOfGroupMessagesAsync()
        {
            int numOfMessage = 329; //await _httpClient.GetFromJsonAsync<int>("api/serverstats/numberOfMessage");


            var data = new List<SystemData>
            {
                new SystemData
                {
                   NumOfUser = numOfMessage,
                   Category = "Num Of Message"
                },
                new SystemData
                {
                   NumOfUser = 5,
                   Category = "Num Of User"
                },
                new SystemData
                {
                   NumOfUser = 2,
                   Category = "Num Of Group"
                }
            };

            return new SystemStats
            {
                Data = data,
                Label = "Group Chat Stats"
            };
        }
        public async Task<SystemStats> NumOfGroupAnonymousMessagesAsync()
        {
            int numOfMessage = 478; //await _httpClient.GetFromJsonAsync<int>("api/serverstats/numberOfMessage");


            var data = new List<SystemData>
            {
                new SystemData
                {
                   NumOfUser = numOfMessage,
                   Category = "Num Of Message"
                },
                new SystemData
                {
                   NumOfUser = 3,
                   Category = "Num Of User"
                }
            };

            return new SystemStats
            {
                Data = data,
                Label = "Anonymous Chat Stats"
            };
        }
    }
}
