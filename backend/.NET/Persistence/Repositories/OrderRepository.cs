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
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<OrderDto>> GetAllOrderAsync(CancellationToken cancellationToken = default)
        {
            var orderList = await _context.Orders.Include(p => p.OrderItems).Include(p=>p.OrderStatuses).Include(p=>p.address).ToListAsync(cancellationToken);          
            var orderDTOList = orderList.Select(order => new OrderDto
            {
                Id = order.Id,
                AddressLine = order.Address,
                PhoneNumber = order.PhoneNumber,
                Note = order.Note,
                Paymethod = order.Paymethod,
                Amount = order.Amount,
                CustomerName = order.CustomerName,
                DateCreated = order.DateCreated,
                PassivedDate = order.PassivedDate,
                ModifiedDate = order.ModifiedDate,
                UserId = order.UserId,
                EmployeeId = order.EmployeeId,
                VoucherId = order.VoucherId,
                AddressId = order.AddressId,
                CurrentStatus = order.CurrentStatus,
                User = new UserDto
                {
                    FullName = order.address?.FullName,
                    PhoneNumber = order.address?.PhoneNumber,
                },
                OrderStatuses = order.OrderStatuses.Select(item => new OrderStatusDto
                {
                    OrderId = item.OrderId,
                    Status = item.Status

                }).ToList(),
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
            return await _context.Orders.Include(p => p.OrderItems).Include(p=>p.OrderStatuses).FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<List<OrderByUserIdDto>> SelectByUserId(string userId)
        {
            var orders = from o in _context.Orders
                         join oi in _context.OrderItems
                         on o.Id equals oi.OrderId
                         join os in _context.OrderStatuses
                         on o.Id equals os.OrderId
                         join p in _context.Products
                         on oi.ProductId equals p.Id
                         join c in _context.Colors
                         on oi.ColorId equals c.Id
                         join s in _context.Sizes
                         on oi.SizeId equals s.Id
                         join pi in _context.ProductImages
                         on p.Id equals pi.ProductId
                         where pi.ColorId == c.Id
                         where o.UserId == userId
                      
                         select new OrderByUserIdDto
                         {
                             OrderId = o.Id,
                             ProductId = p.Id,
                             ProductName = p.Name,
                             Discount = p.DiscountRate,
                             Price = p.RetailPrice,
                             Quantity = oi.Quantity,
                             ProductImge = pi.ImageUrl,
                             SizeNumber = s.NumberSize,
                             ColorName = c.Name
                         };

            return await orders.ToListAsync();
        }
    }
}