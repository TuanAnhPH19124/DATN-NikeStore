using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.AddAsync(order);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (System.Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<OrderDto>> GetAllOrderAsync(CancellationToken cancellationToken = default)
        {
            var orderList = await _context.Orders.Include(p => p.OrderItems).ToListAsync(cancellationToken);

            var orderDTOList = orderList.Select(order => new OrderDto
            {
                Id = order.Id,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                Status = order.Status,
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
                    OrderDate = item.OrderDate,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
            return orderDTOList;
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
                    currentOrder.Status = order.Status;
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