namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProducts();
    Task<Product> GetProductById(int id);
    Task<Product> AddProduct(AddProductRequest request);
}