using Application.Abstraction.Contracts;
using Application.DTOs.Manufacture;
using Application.DTOs.Parts;
using Application.DTOs.Equipment;
using Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IPartService
    {
        Task<IServiceResult<Pagination<PartDto>>> GetPartsAsync(int? page, int? pageSize);
        Task<IServiceResult<PartCreateDto>> AddPartAsync(PartCreateDto partCreateDto);
        Task<IServiceResult<EquipmentAndPartDto>> AssignPartToEquipmentAsync(int partId, int equipmentId);
        Task<IServiceResult<PartDto>> IncreasePartQuantityAsync(int partId, int quantityToAdd);
    }

}
