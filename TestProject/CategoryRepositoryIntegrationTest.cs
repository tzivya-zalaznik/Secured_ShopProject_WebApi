using Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class CategoryRepositoryIntegrationTest : IClassFixture<DatabaseFixture>
    {
        private readonly AdoNetUsers214956807Context _dbContext;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _categoryRepository = new CategoryRepository(_dbContext);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsAllCategories()
        {
            // Arrange
            var category1 = new Category { CategoryName = "Category1"};
            var category2 = new Category { CategoryName = "Category2"};
            await _dbContext.Categories.AddRangeAsync(category1, category2);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetALlCategories();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.CategoryName == "Category1");
            Assert.Contains(result, c => c.CategoryName == "Category2");
        }
    }
}