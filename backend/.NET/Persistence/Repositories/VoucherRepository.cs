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
            _appDbContext = appDbContext;
        }
        public async Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken = default)
        {
            var voucherList = await _appDbContext.Vouchers.ToListAsync(cancellationToken);
            return voucherList;
        }

        public async Task<Voucher> GetByIdVoucherAsync(string id, CancellationToken cancellationToken = default)
        {
            var voucher = await _appDbContext.Vouchers.FirstOrDefaultAsync(e => e.Id == id);
            return voucher;
        }

        public async Task<List<Voucher>> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Vouchers.Where(p => p.Code.ToLower() == code.ToLower()).ToListAsync();
        }

        public async Task AddVoucher(Voucher voucher)
        {
            await _appDbContext.Vouchers.AddAsync(voucher);
            _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateVoucher(string id, Voucher voucher)
        {
            _appDbContext.Vouchers.Update(voucher);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
