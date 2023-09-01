using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.User;
using EntitiesDto;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapster;

namespace Service
{
    public class VoucherService : IVoucherService
    {
        private readonly IRepositoryManger _repositoryManger;

        public VoucherService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }
        public async Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken = default)
        {
            List<Voucher> voucherList = await _repositoryManger.VoucherRepository.GetAllVoucherAsync (cancellationToken);
            return voucherList;
        }

        public async Task<Voucher> GetByIdVoucherAsync(string id, CancellationToken cancellationToken = default)
        {
            Voucher voucher = await _repositoryManger.VoucherRepository.GetByIdVoucherAsync(id, cancellationToken);
            return voucher;
        }
        public async Task<Voucher> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _repositoryManger.VoucherRepository.GetByCodeAsync(code, cancellationToken);
        }
        public async Task<Voucher> CreateAsync(Voucher voucher)
        {
            await _repositoryManger.VoucherRepository.AddVoucher(voucher);
            // await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return voucher;
        }
        public async Task<Voucher> UpdateByIdVoucher(string id, Voucher voucher, CancellationToken cancellationToken = default)
        {
            var existingVoucher = await _repositoryManger.VoucherRepository.GetByIdVoucherAsync(id, cancellationToken);
            if (existingVoucher == null)
            {
                throw new Exception("User not found.");
            }
            else
            {
                existingVoucher.Code = voucher.Code;
                existingVoucher.Value = voucher.Value;
                existingVoucher.Description = voucher.Description;
                existingVoucher.Quantity = voucher.Quantity;
                existingVoucher.StartDate = voucher.StartDate;
                existingVoucher.EndDate = voucher.EndDate;
                existingVoucher.Status = voucher.Status;
                existingVoucher.CreatedDate= voucher.CreatedDate;
                await _repositoryManger.VoucherRepository.UpdateVoucher(id, existingVoucher);
                return existingVoucher;
            }
        }
    }
}
