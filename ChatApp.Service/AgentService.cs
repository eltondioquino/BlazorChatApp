using ChatApp.Domain.Entities;
using ChatApp.Repository;
using ChatApp.Repository.Interface;
using ChatApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepository;

        public AgentService(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public async Task<List<Agent>> GetAvailableAgentsAsync()
        {
            return await _agentRepository.GetAvailableAgentsAsync();
        }

        public async Task UpdateAgentAsync(Agent agent)
        {
            await _agentRepository.UpdateAgentAsync(agent);
        }

        public async Task<List<Agent>> GetAllAgentsAsync()
        {
            return await _agentRepository.GetAllAgentsAsync();
        }

        public async Task<List<(Agent Agent, Shift Shift, Team Team)>> GetAgentShiftTeams()
        {
            return await _agentRepository.GetAgentShiftTeams();
        }

        public async Task<List<Agent>> GetAgentsWithChatAsync()
        {
            return await _agentRepository.GetAgentsWithChatAsync();
        }
    }
}
