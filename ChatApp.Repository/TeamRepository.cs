using ChatApp.Domain.Entities;
using ChatApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private static readonly List<Shift> Shifts = InitialData.GenerateShifts();
        private static readonly List<Agent> Agents = InitialData.GenerateAgents(Shifts);
        private static readonly List<Team> Teams = InitialData.GenerateTeams(Agents);

        public Task<List<Team>> GetAllTeamsAsync()
        {
            return Task.FromResult(Teams);
        }
    }
}
