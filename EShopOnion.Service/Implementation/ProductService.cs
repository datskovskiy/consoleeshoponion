using System.Collections.Generic;
using System.Linq;
using System;
using EShopOnion.Service.Interfaces;
using EShopOnion.DataAccess.Entities;
using EShopOnion.Repository.Interfaces;

namespace EShopOnion.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _productRepository = unitOfWork.Repository<Product>();
        }
        public void CreateProduct(string productName, string category, string description, decimal price)
        {
            if (productName == null)
                throw new ArgumentNullException(nameof(productName), "cant be null.");

            if (productName.Trim() == string.Empty)
                throw new ArgumentException("Product`s name cant be empty.");

            int maxIndex = _productRepository.List().Any() ? 0 : _productRepository.List().Max(u => u.Id);

            var product = new Product
            {
                Id = maxIndex + 1,
                Name = productName,
                Description = description,
                Category = category,
                Price = price
            };

            _productRepository.Create(product);
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product is null)
                return;

            product.IsDeleted = true;

            _productRepository.Update(product);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.List(p => p.Id == id && !p.IsDeleted).FirstOrDefault();
        }

        public Product GetProductByName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "cant be null.");

            if (name.Trim() == string.Empty)
                throw new ArgumentException("Product`s name cant be empty.");

            return _productRepository.List(p => p.Name == name && !p.IsDeleted).FirstOrDefault();
        }

        public IReadOnlyList<Product> GetProducts()
        {
            return _productRepository.List(p => !p.IsDeleted).ToList();
        }

        public void UpdateProduct(int id, string productName, string category, string description, decimal price)
        {
            var product = GetProductById(id);
            if (product is null)
                return;

            product.Name = productName;
            product.Category = category;
            product.Description = description;
            product.Price = price;
        }
    }
}
