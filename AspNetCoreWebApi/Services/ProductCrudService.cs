using AspNetCoreWebApi.Data;
using AspNetCoreWebApi.Models;

namespace AspNetCoreWebApi.Services
{
    /// <summary>
    /// Service class for providing CRUD functions for maintaining product data.
    /// </summary>
    public class ProductCrudService
    {
        private readonly ProductData _productData;

        public ProductCrudService(ProductData productData)
        {
            _productData = productData;
        }

        public List<MasterProduct> GetProducts()
        {
            return _productData.Products;
        }

        /// <summary>
        /// Add new product to the static list.
        /// </summary>
        /// <param name="newProduct"></param>
        public Guid AddNewProduct(NewProduct newProduct)
        {
            var newId = Guid.NewGuid();

            _productData.Products.Add(new MasterProduct
            {
                ProductId = newId,
                Name = newProduct.Name!,
                Price = newProduct.Price,
                Quantity = newProduct.Quantity
            });

            return newId;
        }

        /// <summary>
        /// Get product based on the Product ID.
        /// Return null if not found.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public MasterProduct? GetProduct(Guid productId)
        {
            var product = _productData.Products
                .FirstOrDefault(Q => Q.ProductId == productId);

            return product;
        }
    }
}
