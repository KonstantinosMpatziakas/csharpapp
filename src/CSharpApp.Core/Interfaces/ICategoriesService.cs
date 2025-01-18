using CSharpApp.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharpApp.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<IReadOnlyCollection<Category>> GetCategories();
        Task<Category> GetCategoryById(int category);
        Task<Category> AddCategory(string name, string imageUrl);
        Task<Category> UpdateCategory(int id, string name, string imageUrl);
    }
}
