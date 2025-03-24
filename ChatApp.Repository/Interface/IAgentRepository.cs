using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository.Interface
{
    public interface IAgentRepository
    {
        Task<List<Agent>> GetAvailableAgentsAsync();
        Task UpdateAgentAsync(Agent agent);
        Task<List<Agent>> GetAllAgentsAsync();
        Task<List<(Agent Agent, Shift Shift, Team Team)>> GetAgentShiftTeams();
        Task<List<Agent>> GetAgentsWithChatAsync();
    }
}
