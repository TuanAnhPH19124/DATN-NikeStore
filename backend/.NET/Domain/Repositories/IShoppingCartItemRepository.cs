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
        Task AddRange(List<ShoppingCartItems> items);
        Task<IEnumerable<ShoppingCartItems>> GetAllById(string userId);
        Task<ShoppingCartItems> GetById(string id);

        Task<ShoppingCartItems> GetByRelationId(string userId, string productId, string colorId, string sizeId);

        Task<IEnumerable<ShoppingCartItems>> GetByUserId(string userId);

        void Update(ShoppingCartItems item);
        void UpdateRange (List<ShoppingCartItems> items);

        void Delete(ShoppingCartItems item);

        void DeleteRange(List<ShoppingCartItems> items);
    }
}
