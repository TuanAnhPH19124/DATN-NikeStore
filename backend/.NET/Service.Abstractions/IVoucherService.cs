﻿using Domain.Entities;
using EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IVoucherService
    {
        Task<Voucher> CreateAsync(Voucher voucher);       
        Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken = default);
        Task<Voucher> GetByIdVoucher(Guid id, CancellationToken cancellationToken = default);
        Task<Voucher> UpdateByIdVoucher(Guid id, Voucher voucher, CancellationToken cancellationToken = default);
    }
}
