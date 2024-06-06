using Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text.Json;



namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private AdoNetUsers214956807Context _categoriesContext;
        public CategoryRepository(AdoNetUsers214956807Context categoriesContext)
        {
            _categoriesContext = categoriesContext;
        }
        public async Task<List<Category>> GetALlCategories()
        {
            return await _categoriesContext.Categories.ToListAsync();
        }
    }
}
