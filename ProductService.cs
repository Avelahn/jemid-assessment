using ProductAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProductAPI.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductService(IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        MongoClient mongoClient = new MongoClient(productDatabaseSettings.Value.ConnectionString);

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(productDatabaseSettings.Value.DatabaseName);

        _productCollection = mongoDatabase.GetCollection<Product>(productDatabaseSettings.Value.ProductsCollectionName);
    }

    public async Task<List<Product>> GetAsync(ProductQueryParams queryParams)
    {
        List<Product> products = await _productCollection.Find(_ => true).ToListAsync();

        // Filter products by attribute
        if (queryParams.name != null) {
            string name = queryParams.name.Trim().ToLower(); // clean user input
            products = products.FindAll(p => p.Name.ToLower().Contains(name));
        }
        if (queryParams.minSize != null) {
            products = products.FindAll(p => p.PotSize >= queryParams.minSize);
        }
        if (queryParams.maxSize != null) {
            products = products.FindAll(p => p.PotSize <= queryParams.maxSize);
        }
        if (queryParams.color != null) {
            products = products.FindAll(p => p.Color == queryParams.color);
        }
        if (queryParams.category != null) {
            products = products.FindAll(p => p.ProductCategory == queryParams.category);
        }

        // Sort products by order/field
        if (queryParams.sortOrder != null && queryParams.sortField != null) {
            if (queryParams.sortOrder == "ASC") {
                if (queryParams.sortField == "name") {
                    products = products.OrderBy(p => p.Name).ToList();
                } else
                if (queryParams.sortField == "potSize") {
                    products = products.OrderBy(p => p.PotSize).ToList();
                } else
                if (queryParams.sortField == "plantHeight") {
                    products = products.OrderBy(p => p.PlantHeight).ToList();
                }
            } else
            if (queryParams.sortOrder == "DESC") {
                if (queryParams.sortField == "name") {
                    products = products.OrderByDescending(p => p.Name).ToList();
                } else
                if (queryParams.sortField == "potSize") {
                    products = products.OrderByDescending(p => p.PotSize).ToList();
                } else
                if (queryParams.sortField == "plantHeight") {
                    products = products.OrderByDescending(p => p.PlantHeight).ToList();
                }
            }
        }

        // Page products (default 1:10)
        return products
            .Skip((queryParams.pageNumber - 1) * queryParams.pageSize)
            .Take(queryParams.pageSize)
            .ToList();
    }

    public async Task<Product?> GetAsync(string id) =>
        await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _productCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productCollection.DeleteOneAsync(x => x.Id == id);
}
