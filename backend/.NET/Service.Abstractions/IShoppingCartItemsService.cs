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
        Task UpdateCartItemAsync(string Id, ShoppingCartItems shoppingCartItems);
        Task UpdatePutAsync(string Id, Boolean isQuantity);
        Task RemoveProductFromCartItemAsync(string cartItemId, string productId);
        Task<ShoppingCartItems> checkProduct(string productId, string ShoppingCartId);
    }
}
