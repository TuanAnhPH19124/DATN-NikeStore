using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IShoppingCartItemRepository
    {
        Task Add(ShoppingCartItems item);

        Task<ShoppingCartItems> GetById(string id);

        Task<ShoppingCartItems> GetByUserIdAndStockId(string userId, string stockId);

        Task<IEnumerable<ShoppingCartItems>> GetByUserId(string userId);

        void Update(ShoppingCartItems item);

        void Delete(ShoppingCartItems item);
    }
}
