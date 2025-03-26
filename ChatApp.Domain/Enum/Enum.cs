using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Enum
{
    public class Enum
    {
        public enum AgentLevel
        {
            [Display(Name = "Junior")]
            Junior = 1,

            [Display(Name = "Mid-Level")]
            MidLevel = 2,

            [Display(Name = "Senior")]
            Senior = 3,

            [Display(Name = "Team Lead")]
            TeamLead = 4
        }

        public enum ChatStatus
        {
            Pending,
            Assigned,
            UnAssigned,
            Inprogress,
            Inactive,
            Completed
        }

        public enum ShiftSchedule
        {
            [Display(Name = "Day Shift")]
            DayShift,

            [Display(Name = "Mid Shift")]
            MidShift,

            [Display(Name = "Night Shift")]
            NightShift,

            [Display(Name = "OverFlow")]
            Overflow
        }
    }
   
}
