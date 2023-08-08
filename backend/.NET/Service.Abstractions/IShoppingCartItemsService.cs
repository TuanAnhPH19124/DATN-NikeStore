using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IShoppingCartItemsService
    {
        Task<ShoppingCarts> GetByUserIdAsync(string userId);
        Task AddCartItemAsync(ShoppingCartItems item);
        Task UpdateCartItemAsync(ShoppingCartItems item);
        Task DeleteCartItemAsync(string  productId);
    }
}
