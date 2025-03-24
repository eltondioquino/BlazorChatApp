using ChatApp.Domain.Entities;
using ChatApp.Repository;
using ChatApp.Repository.Interface;
using ChatApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftService(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public async Task<Shift?> GetShiftById(int id)
        {
            return await _shiftRepository.GetShiftByIdAsync(id);
        }

        public async Task<List<Shift>> GetAllShifts()
        {
            return await _shiftRepository.GetAllShiftsAsync();
        }
    }
}
