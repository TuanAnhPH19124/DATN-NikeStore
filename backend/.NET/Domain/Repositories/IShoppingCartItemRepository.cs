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
        Task<ShoppingCarts> GetByUserIdAsync(string userId);
        void AddCartItemAsync(ShoppingCartItems item);
        void UpdateCartItemAsync(ShoppingCartItems item);
        void DeleteCartItemAsync(string productId);
    }
}
