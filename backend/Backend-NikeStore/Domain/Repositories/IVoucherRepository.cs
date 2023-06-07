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
        void AddVoucher(Voucher voucher);
        void UpdateVoucher(Voucher voucher);
        Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken = default);
        Task<Voucher> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
