using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        Task AddItemToBasket(int productId, int quantity = 1);
    }
}