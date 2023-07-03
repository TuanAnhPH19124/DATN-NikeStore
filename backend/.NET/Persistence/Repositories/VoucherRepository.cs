using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class VoucherRepository : IVoucherRepository
    {
        private readonly AppDbContext _appDbContext;
        public VoucherRepository(AppDbContext appDbContext)
        {
            _appDbContext= appDbContext;
        }

        public async Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken = default)
        {
            List<Voucher> voucherList = await _appDbContext.Vouchers.ToListAsync(cancellationToken);
            return voucherList;
        }
        public async Task<Voucher> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {           
            var voucher = await _appDbContext.Vouchers.FirstOrDefaultAsync(e => e.Id == id);
            return voucher;
        }

        public async void AddVoucher(Voucher voucher)
        {
            _appDbContext.Vouchers.Add(voucher);
        }

        public async void UpdateVoucher(Voucher voucher)
        {
            _appDbContext.Vouchers.Update(voucher);
        }

    }
}
