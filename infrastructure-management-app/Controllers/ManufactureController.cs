﻿using Application.Abstraction.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Services;

namespace infrastructure_management_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufactureController : ControllerBase
    {
        private readonly IManufactureService _manufactureService;

        public ManufactureController(IManufactureService manufactureService)
        {
            _manufactureService = manufactureService;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetManufactures(int? pageSize, int? pageNumber)
        {

            var result = _manufactureService.GetManufacturesAsync(pageSize, pageNumber).Result;
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
            
        }

        [HttpGet("DropDown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetManufacturesforInput(string? name)
        {
            var result = _manufactureService.GetManufactureForInput(name).Result;
            return Ok(result.Data);

        }
    }
}
