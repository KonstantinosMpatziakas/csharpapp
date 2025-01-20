using System.Text;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IOptions<RestApiSettings> restApiSettings, 
        ILogger<ProductsService> logger, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        var client = _httpClientFactory.CreateClient("productsApi");

        var response = await client.GetAsync(_restApiSettings.Products!);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<List<Product>>(content);
        
        return res.AsReadOnly();
    }

    public async Task<Product> GetProductById(int id)
    {
        var client = _httpClientFactory.CreateClient("productsApi");

        var response = await client.GetAsync($"{_restApiSettings.Products!}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status {response.StatusCode}: {errorContent}");
        }
        //response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<Product>(content);

        return res!;
    }

    public async Task<Product> AddProduct(AddProductRequest request)
    {
        var client = _httpClientFactory.CreateClient("productsApi");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var jsonContent = JsonSerializer.Serialize<AddProductRequest>(request, options);
        var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(_restApiSettings.Products!, data);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status {response.StatusCode}: {errorContent}");
        }
        //response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<Product>(content);

        return res!;
    }

    public async Task<Category> UpdateProduct(int id, UpdateProductRequest request)
    {
       
        var client = _httpClientFactory.CreateClient("productsApi");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var jsonContent = JsonSerializer.Serialize<UpdateProductRequest>(request);
        var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"{_restApiSettings.Products!}/{id}", data);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status {response.StatusCode}: {errorContent}");
        }
        //response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<Category>(content);

        return res!;
    }
}