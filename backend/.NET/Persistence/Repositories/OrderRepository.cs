using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Post(Order order)
        {
            await _context.AddAsync(order);
        }

        public async Task<List<OrderDto>> GetAllOrderAsync(CancellationToken cancellationToken = default)
        {
            var orderList = await _context.Orders.Include(p => p.OrderItems).ToListAsync(cancellationToken);

            var orderDTOList = orderList.Select(order => new OrderDto
            {
                Id = order.Id,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                Note = order.Note,
                Paymethod = order.Paymethod,
                Amount = order.Amount,
                CustomerName = order.CustomerName,
                DateCreated = order.DateCreated,
                PassivedDate = order.PassivedDate,
                ModifiedDate = order.ModifiedDate,
                UserId = order.UserId,
                VoucherId = order.VoucherId,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    ColorId = item.ColorId,
                    SizeId = item.SizeId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
            return orderDTOList;
        }

        public async Task<Order> GetByIdOrderAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.Include(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Order> SelectById(string id)
        {
            return await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<string> Update(string id, Order order)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var currentOrder = await SelectById(id);
                    currentOrder.ModifiedDate = order.ModifiedDate;
                    _context.Orders.Update(currentOrder);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return currentOrder.Id;
                }
                catch (System.Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}