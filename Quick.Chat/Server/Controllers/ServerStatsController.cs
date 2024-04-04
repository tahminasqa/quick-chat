using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quick.Chat.Server.Data;
using Quick.Chat.Shared;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quick.Chat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServerStatsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServerStatsController> _logger;

        public ServerStatsController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<ServerStatsController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        [HttpGet("registeredusers")]
        public async Task<IActionResult> GetActiveUsersAsync()
        {
            this._logger.LogInformation($">> [ServerStatsController][GetUsersAsync]");

            int numberOf = await _context.Users.CountAsync();
            this._logger.LogInformation($">> [ServerStatsController][GetUsersAsync][Total User Found: {numberOf}]");
            return Ok(numberOf);
        }

        [HttpGet("numberOfMessage")]
        public async Task<IActionResult> GetNumberOfmessageAsync()
        {
            this._logger.LogInformation($">> [ServerStatsController][GetNumberOfmessageAsync]");
            int numberOf = await _context.ChatMessages.CountAsync();
            this._logger.LogInformation($">> [ServerStatsController][GetNumberOfmessageAsync][Total message Found: {numberOf}]");
            return Ok(numberOf);
        }
    }
}
