using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository.Interface
{
    public interface IShiftRepository
    {
        Task<List<Shift>> GetAllShiftsAsync();

        Task<Shift?> GetShiftByIdAsync(int id);
    }
}
