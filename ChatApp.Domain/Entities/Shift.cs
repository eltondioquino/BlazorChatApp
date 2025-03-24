using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Domain.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        //public string Name { get; set; } // e.g., Day Shift, Night Shift, Overflow
        public ShiftSchedule Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive => DateTime.UtcNow.TimeOfDay >= StartTime && DateTime.UtcNow.TimeOfDay <= EndTime;
        public List<Agent> Agents { get; set; } = new List<Agent>();
    }

}
