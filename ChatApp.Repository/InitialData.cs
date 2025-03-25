using ChatApp.Domain.Entities;
using ChatApp.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Repository
{
    public static class InitialData
    {
        public static List<Shift> GenerateShifts()
        {
            return new List<Shift>
            {
                new Shift { Name = ShiftSchedule.DayShift, StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
                new Shift { Name = ShiftSchedule.MidShift, StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(22, 0, 0) },
                new Shift { Name = ShiftSchedule.NightShift, StartTime = new TimeSpan(22, 0, 0), EndTime = new TimeSpan(6, 0, 0) },
                new Shift { Name = ShiftSchedule.Overflow, StartTime = TimeSpan.Zero, EndTime = TimeSpan.Zero }
            };
        }

        public static List<Agent> GenerateAgents(List<Shift> shifts)
        {
            var agents = new List<Agent>();

            // Team A (Day Shift)
            agents.Add(new Agent { Id = 1, Name = "Agent A1", AgentLevel = AgentLevel.TeamLead, CurrentShift = shifts[0] });
            agents.Add(new Agent { Id = 2, Name = "Agent A2", AgentLevel = AgentLevel.MidLevel, CurrentShift = shifts[0] });
            agents.Add(new Agent { Id = 3, Name = "Agent A3", AgentLevel = AgentLevel.MidLevel, CurrentShift = shifts[0] });
            agents.Add(new Agent { Id = 4, Name = "Agent A4", AgentLevel = AgentLevel.Junior, CurrentShift = shifts[0] });

            // Team B (Mid Shift)
            agents.Add(new Agent { Id = 5, Name = "Agent B1", AgentLevel = AgentLevel.Senior, CurrentShift = shifts[1] });
            agents.Add(new Agent { Id = 6, Name = "Agent B2", AgentLevel = AgentLevel.MidLevel, CurrentShift = shifts[1] });
            agents.Add(new Agent { Id = 7, Name = "Agent B3", AgentLevel = AgentLevel.Junior, CurrentShift = shifts[1] });
            agents.Add(new Agent { Id = 8, Name = "Agent B4", AgentLevel = AgentLevel.Junior, CurrentShift = shifts[1] });

            // Team C (Night Shift)
            agents.Add(new Agent { Id = 9, Name = "Agent C1", AgentLevel = AgentLevel.MidLevel, CurrentShift = shifts[2] });
            agents.Add(new Agent { Id = 10, Name = "Agent C2", AgentLevel = AgentLevel.MidLevel, CurrentShift = shifts[2] });

            // Overflow Team (Always Available)
            for (int i = 11; i <= 16; i++)
            {
                agents.Add(new Agent { Id = i, Name = $"Overflow Agent {i}", AgentLevel = AgentLevel.Junior });
            }

            return agents;
        }

        public static List<Team> GenerateTeams(List<Agent> agents)
        {
            return new List<Team>
            {
                new Team { Name = "Team A", Agents = agents.Where(a => a.Id <= 4).ToList() },
                new Team { Name = "Team B", Agents = agents.Where(a => a.Id >= 5 && a.Id <= 8).ToList() },
                new Team { Name = "Team C", Agents = agents.Where(a => a.Id >= 9 && a.Id <= 10).ToList() }
            };
        }

        public static List<(Agent Agent, Shift Shift, Team Team)> GetAgentShiftTeams()
        {
            var shifts = GenerateShifts();
            var agents = GenerateAgents(shifts);
            var teams = GenerateTeams(agents);

            var agentShiftTeams = new List<(Agent, Shift, Team)>();

            foreach (var team in teams)
            {
                foreach (var agent in team.Agents)
                {
                    var shift = agent.CurrentShift ?? new Shift { Name = ShiftSchedule.Overflow, StartTime = TimeSpan.Zero, EndTime = TimeSpan.Zero };
                    agentShiftTeams.Add((agent, shift, team));
                }
            }

            return agentShiftTeams;
        }

        public static List<User> GenerateUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PasswordHash = "hashedpassword123",
                    PhoneNumber = "123-456-7890",
                    ProfilePictureUrl = "https://example.com/profile/john_doe.png",
                    CreatedAt = DateTime.UtcNow,
                    Role = "User",
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PasswordHash = "securepassword456",
                    PhoneNumber = "987-654-3210",
                    ProfilePictureUrl = "https://example.com/profile/jane_smith.png",
                    CreatedAt = DateTime.UtcNow,
                    Role = "Agent",
                    IsActive = true
                },
                new User
                {
                    Id = 3,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    PasswordHash = "mypassword789",
                    PhoneNumber = "555-123-4567",
                    ProfilePictureUrl = "https://example.com/profile/alice_johnson.png",
                    CreatedAt = DateTime.UtcNow,
                    Role = "Admin",
                    IsActive = true
                }
            };
        }
    }
}
