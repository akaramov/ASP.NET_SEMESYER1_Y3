using APAERMENT_LAST_API.DTOs.Requests;
using APAERMENT_LAST_API.DTOs.Responses;
using APAERMENT_LAST_API.Exceptions;
using APAERMENT_LAST_API.Helpers;
using APAERMENT_LAST_API.Models;
using APAERMENT_LAST_API.Repositories.Interfaces;
using APAERMENT_LAST_API.Services.Interface;
using AutoMapper;

namespace APAERMENT_LAST_API.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repository;
        private readonly IMapper _mapper;

        public BuildingService(IBuildingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<BuildingResDto>> GetBuildings(int page = 1, int pageSize = 10)
        {
            var buildings = await _repository.GetBuildings(page, pageSize);
            if (buildings == null)
            {
                throw new NotFoundException("Data not found.");
            }
            return new PagedResult<BuildingResDto>
            {
                PageNumber = buildings.PageNumber,
                PageSize = buildings.PageSize,
                TotalRecords = buildings.TotalRecords,
                TotalPages = buildings.TotalPages,
                Data = _mapper.Map<List<BuildingResDto>>(buildings.Data),
            };
        }

        public async Task<BuildingResDto> GetBuilding(int id)
        {
            if (id == 0)
            {
                throw new ValidationException(
                    new List<string>
                    {
                    "Id must greater than Zero."
                    }
                );
            }
            var building = await _repository.GetBuilding(id);
            if (building == null)
            {
                throw new NotFoundException("Data not found.");
            }
            return _mapper.Map<Building, BuildingResDto>(building);
        }
        public async Task<BuildingResDto> Create(BuildingReqDto buildingDto)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(buildingDto.NameKhmer))
            {
                errors.Add("Name khmer is required.");
            }
            if (string.IsNullOrEmpty(buildingDto.NameEnglish))
            {
                errors.Add("Name english is required.");
            }
            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
            var data = _mapper.Map<Building>(buildingDto);
            var building = await _repository.Create(data);
            return _mapper.Map<Building, BuildingResDto>(building);
        }
        public async Task<BuildingResDto> Update(int id, BuildingReqDto buildingDto)
        {
            var entity = await _repository.GetBuilding(id);

            if (entity == null)
            {
                throw new NotFoundException("Not found");
            }

            var errors = new List<string>();
            if (string.IsNullOrEmpty(buildingDto.NameKhmer))
            {
                errors.Add("Name khmer is required.");
            }
            if (string.IsNullOrEmpty(buildingDto.NameEnglish))
            {
                errors.Add("Name english is required.");
            }
            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

            _mapper.Map(buildingDto, entity);
            var building = await _repository.Update(entity);
            return _mapper.Map<Building, BuildingResDto>(building);
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _repository.GetBuilding(id);
            if (entity == null)
            {
                throw new NotFoundException("Not found");
            }
            await _repository.Delete(entity);
            return true;
        }
    }
}
