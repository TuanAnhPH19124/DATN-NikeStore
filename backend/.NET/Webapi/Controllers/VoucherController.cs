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

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public VoucherController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVoucher()
        {
            try
            {
                var vouchers = await _serviceManager.VoucherService.GetAllVoucherAsync();
                if (vouchers == null || !vouchers.Any())
                {
                    return NotFound();
                }
                return Ok(vouchers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(string Id)
        {
            var voucher = await _serviceManager.VoucherService.GetByIdVoucher(Id);

            if (voucher == null)
            {
                return NotFound();
            }
            return voucher;
        }

        [HttpPost]
        public async Task<ActionResult<Voucher>> CreateVoucher(Voucher voucher)
        {
            try
            {              
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdVoucher = await _serviceManager.VoucherService.CreateAsync(voucher);         
                return CreatedAtAction(nameof(GetVoucher), new { id = createdVoucher.Id }, createdVoucher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(string id, Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            { 
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
