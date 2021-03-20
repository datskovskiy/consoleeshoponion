using EShopOnion.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopOnion.Service.Interfaces
{
    public interface IProductService
    {
        Product GetProductById(int id);

        Product GetProductByName(string name);

        IReadOnlyList<Product> GetProducts();

        void CreateProduct(string productName, string category, string description, decimal price);

        void UpdateProduct(int id, string productName, string category, string description, decimal price);

        void DeleteProduct(int id);
    }
}
