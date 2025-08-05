using Application.Abstraction.Contracts;
using Application.Abstraction.Services;
using Application.DTOs.Equipment;
using Application.DTOs.OperationSite;
using Application.DTOs.Parts;
using Application.Repositories.EquipmentPartRepo;
using Application.Repositories.PartRepo;
using Application.RequestParameters;
using AutoMapper;
using Domain.Entities;
using Persistence.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class PartService : IPartService
    {
        private readonly IEquipmentPartReadRepository _equipmentPartReadRepository;
        private readonly IEquipmentPartWriteRepository _equipmentPartWriteRepository;

        private readonly IPartWriteRepository _partWriteRepository;
        private readonly IPartReadRepository _partReadRepository;

        private readonly IMapper _mapper;


        public PartService(
        IEquipmentPartReadRepository equipmentPartReadRepository,
        IEquipmentPartWriteRepository equipmentPartWriteRepository,
        IPartWriteRepository partWriteRepository,
        IPartReadRepository partReadRepository, 
        IMapper mapper)
        {
            _equipmentPartReadRepository = equipmentPartReadRepository;
            _equipmentPartWriteRepository = equipmentPartWriteRepository;
            _partWriteRepository = partWriteRepository;
            _mapper = mapper;
        }

        public async Task<IServiceResult<PartCreateDto>> AddPartAsync(PartCreateDto partCreateDto)
        {
            var part = _mapper.Map<Part>(partCreateDto);
            part.IsActive = true;
            part.IsDeleted = false;
            part.CreatedDate = DateTime.UtcNow;

            await _partWriteRepository.AddAsync(part);
            await _partWriteRepository.SaveAsync();

            return new ServiceResult<PartCreateDto>
            {
                IsSuccess = true,
                Data = partCreateDto
            };
        }

        public async Task<IServiceResult<EquipmentAndPartDto>> AssignPartToEquipmentAsync(int partId, int equipmentId)
        {
            var equipmentPart = new EquipmentPart
            {
                PartId = partId,
                EquipmentId = equipmentId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _equipmentPartWriteRepository.AddAsync(equipmentPart);
            await _equipmentPartWriteRepository.SaveAsync();

            var result = new EquipmentAndPartDto
            {
                EquipmentId = equipmentId,
                PartId = partId
            };

            return new ServiceResult<EquipmentAndPartDto>
            {
                IsSuccess = true,
                Data = result
            };
        }

        public async Task<IServiceResult<Pagination<PartDto>>> GetPartsAsync(int? page, int? pageSize)
        {
            int currentPage = page.GetValueOrDefault(1);
            int currentPageSize = pageSize.GetValueOrDefault(int.MaxValue);

            if (currentPage <= 0 || currentPageSize <= 0)
                return new ServiceResult<Pagination<PartDto>> { IsSuccess = false, ErrorMessage = "Invalid pagination values." };

            var query = _partReadRepository.GetAll(false)
                .Where(p => p.IsActive == true && p.IsDeleted == false);

            var totalCount = query.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount / currentPageSize);

            var items = query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            if (!items.Any())
                return new ServiceResult<Pagination<PartDto>> { IsSuccess = false, ErrorMessage = "No parts found." };

            var partDtos = _mapper.Map<List<PartDto>>(items);
            var pagination = new Pagination<PartDto>(partDtos, currentPage, pageCount, totalCount);

            return new ServiceResult<Pagination<PartDto>> { IsSuccess = true, Data = pagination };
        }

        public async Task<IServiceResult<PartDto>> IncreasePartQuantityAsync(int partId, int quantityToAdd)
        {
            var part = await _partReadRepository.GetByIdAsync(partId);
            if (part == null)
            {
                return new ServiceResult<PartDto>
                {
                    IsSuccess = false,
                    ErrorMessage = "Part not found."
                };
            }

            part.Quantity += quantityToAdd;
            part.UpdatedDate = DateTime.UtcNow;

            _partWriteRepository.Update(part);
            await _partWriteRepository.SaveAsync();

            var dto = _mapper.Map<PartDto>(part);

            return new ServiceResult<PartDto>
            {
                IsSuccess = true,
                Data = dto
            };
        }

        //public async Task<IServiceResult<Pagination<PartDto>>> GetPartsAsync(int? page, int? pageSize)
        //{
        //    if (page == null || pageSize == null)
        //    {

        //        var countt = _readRepository.GetAll().Where(d => d.IsDeleted == false && d.IsActive == true).Count();
        //        int pageValuee = 1;
        //        int takeValuee = countt;

        //        var partss = _readRepository.GetAll();

        //        if (partss == null)
        //        {
        //            return new ServiceResult<Pagination<PartDto>> { IsSuccess = false, ErrorMessage = "There is no data in DB" };
        //        }
        //        var itemss = partss.ToList();
        //        var totalCountt = countt;
        //        var pageCountt = (int)Math.Ceiling((double)totalCountt / takeValuee);
        //        var partsDto = _mapper.Map<List<PartDto>>(itemss);

        //        var paginationn = new Pagination<PartDto>(partsDto, pageValuee, pageCountt, totalCountt);

        //        return  new ServiceResult<Pagination<PartDto>> { IsSuccess = true, Data = paginationn };
        //    }
        //    if ((!page.HasValue || !pageSize.HasValue || page <= 0 || pageSize <= 0))
        //    {
        //        return new ServiceResult<Pagination<PartDto>> { IsSuccess = false, ErrorMessage = "Params is not okay" };
        //    }

        //    int pageValue = page.Value;
        //    int takeValue = pageSize.Value;
        //    //int skipCount = (pageValue > 1) ? (pageValue - 1) * takeValue : 0;
        //    var parts = _readRepository.GetAll(tracking: false);

        //    var items = parts
        //            .Skip((pageValue - 1) * takeValue)
        //            .Take(takeValue)
        //            .Where(e => e.IsDeleted == false && e.IsActive == true )
        //            .ToList();


        //    if (items == null)
        //    {
        //        return new ServiceResult<Pagination<PartDto>> { IsSuccess = false, ErrorMessage = "There is no parts in DB" };
        //    }
        //    var totalCount = _readRepository.GetAll(tracking: false).ToList().Count;
        //    var pageCount = (int)Math.Ceiling((double)totalCount / takeValue);

        //    var partsDtoo = _mapper.Map<List<PartDto>>(items);
        //    var pagination = new Pagination<PartDto>(partsDtoo, pageValue, pageCount, totalCount);

        //    return  new  ServiceResult<Pagination<PartDto>> { IsSuccess = true, Data=pagination};
        //}
    }
}
