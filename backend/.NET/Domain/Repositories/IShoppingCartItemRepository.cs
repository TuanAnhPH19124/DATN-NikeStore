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
        Task UpdateCartItemAsync(string Id, ShoppingCartItems shoppingCartItems);
        Task<ShoppingCartItems> GetByIdCartItemAsync(string Id);
        Task RemoveProductFromCartItemAsync(string cartItemId, string productId);
        Task<ShoppingCartItems> CheckProductAsync(string productId, string ShoppingCartId);
    }
}
