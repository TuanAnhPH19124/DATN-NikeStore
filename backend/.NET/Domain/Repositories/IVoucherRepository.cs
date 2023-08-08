using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IVoucherRepository
    {
        Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken = default);
        Task<Voucher> GetByIdVoucherAsync(string id, CancellationToken cancellationToken = default);
        Task AddVoucher(Voucher voucher);
        Task UpdateVoucher(string id, Voucher voucher);
    }
}
