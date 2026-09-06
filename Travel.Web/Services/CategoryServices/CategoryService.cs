using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var database = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(
                databaseSettings.Value.CategoryCollectionName);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryCollection
                .Find(_ => true)
                .SortBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _categoryCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            category.Name = category.Name.Trim();

            await _categoryCollection.InsertOneAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var existingCategory = await GetByIdAsync(category.Id);

            if (existingCategory == null)
                throw new KeyNotFoundException(
                    $"Kategori bulunamadı. Id: {category.Id}");

            category.Name = category.Name.Trim();

            await _categoryCollection.ReplaceOneAsync(
                x => x.Id == category.Id,
                category);
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            await _categoryCollection.DeleteOneAsync(
                x => x.Id == id);
        }
    }
}