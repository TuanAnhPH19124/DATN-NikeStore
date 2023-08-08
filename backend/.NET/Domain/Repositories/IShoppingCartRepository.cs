using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IShoppingCartRepository
    {
        Task AddAsync(ShoppingCarts cart);
        Task DeleteAsync(string Id);
        Task<bool> CheckUserCart(string userId);
    }
}
