namespace CSharpApp.Application.Categories
{
    public class CategoriesService: ICategoriesService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RestApiSettings _restApiSettings;

        public CategoriesService(IOptions<RestApiSettings> restApiSettings, 
            IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
            _restApiSettings = restApiSettings.Value;
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
    }
}
