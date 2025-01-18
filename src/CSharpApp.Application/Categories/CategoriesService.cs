using CSharpApp.Application.Products;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpApp.Application.Categories
{
    public class CategoriesService: ICategoriesService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger _logger;

        public CategoriesService(IOptions<RestApiSettings> restApiSettings, 
            IHttpClientFactory httpClientFactory, ILogger<ProductsService> logger) 
        {
            _httpClientFactory = httpClientFactory;
            _restApiSettings = restApiSettings.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategories()
        {
            var client = _httpClientFactory.CreateClient("productsApi");

            var response = await client.GetAsync(_restApiSettings.Categories!);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<Category>>(content);

            return res.AsReadOnly();
        }

        public async Task<Category> GetCategoryById(int category)
        {
            var client = _httpClientFactory.CreateClient("productsApi");

            var response = await client.GetAsync($"{_restApiSettings.Categories!}/{category}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Category>(content);

            return res!;
        }

        public async Task<Category> AddCategory(string name, string imageUrl)
        {
            var client = _httpClientFactory.CreateClient("productsApi");

            var category = new Category();

            if (!string.IsNullOrEmpty(name))
            {
                category.Name = name;
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                category.Image = imageUrl;
            }

            var jsonContent = JsonSerializer.Serialize<Category>(category);
            var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_restApiSettings.Categories!, data);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Category>(content);

            return res!;
        }

        public async Task<Category> UpdateCategory(int id, string name, string imageUrl)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("productsApi");

                var category = new Category();

                if (!string.IsNullOrEmpty(name))
                {
                    category.Name = name;
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    category.Image = imageUrl;
                }

                var jsonContent = JsonSerializer.Serialize<Category>(category);
                var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{_restApiSettings.Categories!}/{id}", data);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);

                return res!;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "error");
                throw;
            }
        }
    }
}
