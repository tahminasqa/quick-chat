using Quick.Chat.Server.Data;
using Quick.Chat.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quick.Chat.Server.Hubs;

namespace Quick.Chat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChatController> _logger;

        public ChatController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<ChatController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetConversationAsync(string contactId)
        {
            this._logger.LogInformation($">> [ChatController][ContactId: {contactId}]");
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var messages = await _context.ChatMessages
                    .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatMessage
                    {
                        FromUserId = x.FromUserId,
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUser = x.ToUser,
                        FromUser = x.FromUser
                    }).ToListAsync();
            return Ok(messages);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            this._logger.LogInformation($">> [ChatController][GetUsersAsync]");
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers = await _context.Users.Where(user => user.Id != userId).ToListAsync();
            this._logger.LogInformation($">> [ChatController][GetUsersAsync][Total User Found: {allUsers.Count}]");


            return Ok(allUsers);
        }
        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            this._logger.LogInformation($">> [ChatController][GetAllUsersAsync]");

            //Reset
            ApplicationCache.RegisteredUsers = new System.Collections.Generic.Dictionary<string, string>();
            var registeredUsers = await _context.Users.ToListAsync();
            foreach (var user in registeredUsers)
            {
                ApplicationCache.RegisteredUsers.Add(user.UserName, user.UserName);
            }

            return Ok(ApplicationCache.RegisteredUsers);
        }
        [HttpGet("groupusers")]
        public IActionResult GetGroupUsersAsync()
        {
            this._logger.LogInformation($">> [ChatController][GetGroupUsersAsync]");
            var allUsers = ApplicationCache.GroupUsers!=null? ApplicationCache.GroupUsers:new System.Collections.Generic.Dictionary<string, string>();
            this._logger.LogInformation($">> [ChatController][GetGroupUsersAsync][Total User Found: {allUsers.Count}]");
            return Ok(allUsers);
        }
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            var user = await _context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            this._logger.LogInformation($">> [ChatController][GetUserDetailsAsync][User Id:{userId}][Found User?: {user?.UserName}]");
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> SaveMessageAsync(ChatMessage message)
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;
            message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
            await _context.ChatMessages.AddAsync(message);
            return Ok(await _context.SaveChangesAsync());
        }
    }
}
