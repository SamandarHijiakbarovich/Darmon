using Darmon.Application.DTOs.BranchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<BranchDto> GetByIdAsync(int id);
        Task<BranchDto> CreateAsync(CreateBranchDto dto);
        Task<BranchDto> UpdateAsync(int id, UpdateBranchDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
