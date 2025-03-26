using ChatApp.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Domain.Entities
{
    public class Agent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AgentLevel AgentLevel { get; set; }

        
        public int CurrentChats { get; set; } = 0;
        public int MaxConcurrency => (int)(10 * GetEfficiency(AgentLevel));
        public bool IsOnShift => CurrentShift != null && CurrentShift.IsActive;
        

        private static double GetEfficiency(AgentLevel level) => level switch
        {
            AgentLevel.Junior => 0.4,
            AgentLevel.MidLevel => 0.6,
            AgentLevel.Senior => 0.8,
            AgentLevel.TeamLead => 0.5,
            _ => 0.4,
        };


        public ShiftSchedule Schedule { get; set; }
        public Shift? CurrentShift { get; set; }

        // Renamed to avoid conflict
        public bool IsCurrentlyOnShift => CurrentShift != null && CurrentShift.IsActive;

        // Determines if the agent is active based on their shift or being an overflow agent
        public bool IsActive => IsCurrentlyOnShift || IsOverflowAgent();
        public bool IsAvailable { get; set; }

        // Overflow agents are always active regardless of shift
        private bool IsOverflowAgent() => CurrentShift == null;
    }

}
