using Domain.Entities;
using EntitiesDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using System.Linq.Expressions;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public VoucherController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVoucher()
        {
            try
            {
                var voucher = await _serviceManager.VoucherService.GetAllVoucherAsync();
                if (voucher == null || !voucher.Any())
                {
                    return NotFound();
                }
                return Ok(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("confirm/{code}")]
        public async Task<IActionResult> ConfirmVoucher(string code)
        {
            return Ok();
        }

        [HttpGet("Get/{Id}")]
        public async Task<ActionResult<Voucher>> GetByIdVoucher(string Id)
        {
            var voucher = await _serviceManager.VoucherService.GetByIdVoucherAsync(Id);

            if (voucher == null)
            {
                return NotFound();
            }
            return voucher;
        }
   
        [HttpPost]
        public async Task<IActionResult> CreateVoucher(VoucherDto voucherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVoucher = await _serviceManager.VoucherService.GetByCodeAsync(voucherDto.Code);
            if (existingVoucher != null)
            {
                return Conflict("Code Voucher with the same Name already exists");
            }

            var voucher = new Voucher
            {
                Code = voucherDto.Code,
                Value = voucherDto.Value,
                Expression= voucherDto.Expression,
                Description = voucherDto.Description,
                StartDate = voucherDto.StartDate, 
                EndDate = voucherDto.EndDate,
                Status= voucherDto.Status,
                CreatedDate = voucherDto.CreatedDate
            };
            await _serviceManager.VoucherService.CreateAsync(voucher);
            return CreatedAtAction("GetByIdVoucher", new { id = voucher.Id }, voucher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(string id, VoucherDtoUpdate voucherDtoUpdate)
        {
            if (id != voucherDtoUpdate.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                var voucher = voucherDtoUpdate.Adapt<Voucher>();
                await _serviceManager.VoucherService.UpdateByIdVoucher(id, voucher);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
