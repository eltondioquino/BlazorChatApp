using ChatApp.Domain.Entities;
using ChatApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository
{
    public class ShiftRepository : IShiftRepository
    {
        private static readonly List<Shift> Shifts = InitialData.GenerateShifts();

        public Task<List<Shift>> GetAllShiftsAsync()
        {
            return Task.FromResult(Shifts);
        }
        public Task<Shift?> GetShiftByIdAsync(int id)
        {
            var shift = Shifts.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(shift);
        }

    }
}
