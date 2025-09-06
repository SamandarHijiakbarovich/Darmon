using AutoMapper;
using Darmon.Application.DTOs.BranchDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Darmon.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchService> _logger;

        public BranchService(
            IBranchRepository branchRepository,
            IMapper mapper,
            ILogger<BranchService> logger)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BranchDto> CreateAsync(CreateBranchDto dto)
        {
            try
            {
                var branch = _mapper.Map<Branch>(dto);
                await _branchRepository.AddAsync(branch);
                _logger.LogInformation("Branch created successfully. BranchId: {BranchId}", branch.Id);

                return _mapper.Map<BranchDto>(branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating branch. Data: {@Dto}", dto);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var branch = await _branchRepository.GetByIdAsync(id);
                if (branch == null)
                {
                    _logger.LogWarning("Delete failed. Branch with ID {BranchId} not found", id);
                    return false;
                }

                await _branchRepository.DeleteAsync(branch);
                _logger.LogInformation("Branch deleted successfully. BranchId: {BranchId}", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting branch with ID {BranchId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            try
            {
                var branches = await _branchRepository.GetAllAsync();
                _logger.LogInformation("Retrieved {Count} branches", branches.Count());

                return _mapper.Map<IEnumerable<BranchDto>>(branches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all branches");
                throw;
            }
        }

        public async Task<BranchDto?> GetByIdAsync(int id)
        {
            try
            {
                var branch = await _branchRepository.GetByIdAsync(id);
                if (branch == null)
                {
                    _logger.LogWarning("Branch with ID {BranchId} not found", id);
                    return null;
                }

                _logger.LogInformation("Branch retrieved successfully. BranchId: {BranchId}", id);
                return _mapper.Map<BranchDto>(branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving branch with ID {BranchId}", id);
                throw;
            }
        }

        public async Task<BranchDto?> UpdateAsync(int id, UpdateBranchDto dto)
        {
            try
            {
                var branch = await _branchRepository.GetByIdAsync(id);
                if (branch == null)
                {
                    _logger.LogWarning("Update failed. Branch with ID {BranchId} not found", id);
                    return null;
                }

                _mapper.Map(dto, branch);
                await _branchRepository.UpdateAsync(branch);

                _logger.LogInformation("Branch updated successfully. BranchId: {BranchId}", id);

                return _mapper.Map<BranchDto>(branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating branch with ID {BranchId}. Data: {@Dto}", id, dto);
                throw;
            }
        }
    }
}
