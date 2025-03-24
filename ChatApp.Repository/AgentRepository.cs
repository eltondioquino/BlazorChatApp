using ChatApp.Domain.Entities;
using ChatApp.Repository.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository
{
    public class AgentRepository : IAgentRepository
    {
        //private static readonly ConcurrentBag<Agent> Agents = new();
        private static readonly List<Shift> Shifts = InitialData.GenerateShifts();
        private static readonly List<Agent> Agents = InitialData.GenerateAgents(Shifts);
        private static readonly List<Team> Teams = InitialData.GenerateTeams(Agents);
        private static readonly List<(Agent, Shift, Team)> GetAll = InitialData.GetAgentShiftTeams();
        public Task<List<Agent>> GetAvailableAgentsAsync()
        {
            var availableAgents = Agents.Where(a => a.CurrentChats < a.MaxConcurrency && a.IsActive).ToList();
            return Task.FromResult(availableAgents);
        }

        public Task UpdateAgentAsync(Agent agent)
        {
            var existingAgent = Agents.FirstOrDefault(a => a.Id == agent.Id);
            if (existingAgent != null)
            {
                existingAgent.CurrentChats = agent.CurrentChats;
                //existingAgent.IsActive = agent.IsActive;
            }
            return Task.CompletedTask;
        }

        public Task<List<Agent>> GetAllAgentsAsync()
        {
            return Task.FromResult(Agents.ToList());
        }

        public Task<List<(Agent Agent, Shift Shift, Team Team)>> GetAgentShiftTeams()
        {
            return Task.FromResult(GetAll.ToList());
        }

        public Task<List<Agent>> GetAgentsWithChatAsync()
        {
            var chatAgents = Agents.Where(a => a.CurrentChats > 0).ToList();
            return Task.FromResult(chatAgents);
        }
    }
}
