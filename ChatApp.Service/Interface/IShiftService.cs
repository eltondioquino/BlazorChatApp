using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Service.Interface
{
    public interface IShiftService
    {
        Task<List<Shift>> GetAllShifts();
        Task<Shift?> GetShiftById(int id);
    }
}
