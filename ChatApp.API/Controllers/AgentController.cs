using ChatApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            var agents = await _agentService.GetAllAgentsAsync();
            return Ok(agents);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableAgents()
        {
            var agents = await _agentService.GetAvailableAgentsAsync();
            return Ok(agents);
        }

        [HttpGet("activechat")]
        public async Task<IActionResult> GetAgentsWithChatAsync()
        {
            var agents = await _agentService.GetAgentsWithChatAsync();
            return Ok(agents);
        }

        [HttpGet("GetAgentShiftTeams")]
        public async Task<IActionResult> GetAgentShiftTeams()
        {
            var result = await _agentService.GetAgentShiftTeams();
            if (result == null || !result.Any())
                return NotFound("No agents found.");

            return Ok(result.Select(item => new
            {
                Agent = new
                {
                    item.Agent.Id,
                    item.Agent.Name,
                    item.Agent.AgentLevel
                },
                Shift = new
                {
                    item.Shift.Name,
                    item.Shift.StartTime,
                    item.Shift.EndTime
                },
                Team = new
                {
                    item.Team.Name
                }
            }));
        }
    }
}
