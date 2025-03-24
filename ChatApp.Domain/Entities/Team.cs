using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Agent> Agents { get; set; } = new List<Agent>();
        public int GetCapacity() => Agents.Sum(agent => agent.MaxConcurrency);
    }

}
