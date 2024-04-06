using Microsoft.AspNetCore.Mvc.Testing;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


public class ProductServiceIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductServiceIntegrationTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        // Configurez le client pour utiliser le schéma d'authentification fictif
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
    
}

    [Fact]
    public async Task AddProduct_ByAdmin_ShouldBeAvailableForClient()
    {
        
        // Admin ajoute un nouveau produit
        var newProduct = new { Name = "New Product", Price = 20.00, Quantity = 100 };
        var addResponse = await _client.PostAsync("/Product/Create",
            new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json"));
        addResponse.EnsureSuccessStatusCode();

        // Client récupère la liste des produits pour vérifier la présence du nouveau produit
        var listResponse = await _client.GetAsync("/Product");
        listResponse.EnsureSuccessStatusCode();
        var productList = await JsonSerializer.DeserializeAsync<List<ProductViewModel>>(await listResponse.Content.ReadAsStreamAsync()) ?? new List<ProductViewModel>();
        Assert.Contains(productList, p => p.Name == newProduct.Name && p.Price == newProduct.Price.ToString() && p.Stock == newProduct.Quantity.ToString());

        // Supplémentaire: Client ajoute le produit au panier et vérifie
        // Cette partie dépend de l'implémentation spécifique de votre application
    }

    [Fact]
    public async Task DeleteProduct_ByAdmin_ShouldNotBeAvailableForClient()
    {
       
        // Supposons que le produit à supprimer a l'ID 1
        var productIdToDelete = 1;

        // Admin supprime le produit
        var deleteResponse = await _client.DeleteAsync($"/Product/DeleteProduct?id={productIdToDelete}");
        deleteResponse.EnsureSuccessStatusCode();

        // Client vérifie que le produit n'est plus dans la liste
        var listResponse = await _client.GetAsync("/Product");
        listResponse.EnsureSuccessStatusCode();
        var productList = await JsonSerializer.DeserializeAsync<List<ProductViewModel>>(await listResponse.Content.ReadAsStreamAsync()) ?? new List<ProductViewModel>();
        Assert.DoesNotContain(productList, p => p.Id == productIdToDelete);

        // Supplémentaire: Vérifier que le produit n'est plus dans le panier si il y était
        // Cette partie dépend de l'implémentation spécifique de votre application
    }
}




//using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
//using System.Text;
//using System.Text.Json;


//public class ProductServiceIntegrationTest : IClassFixture<CustomWebApplicationFactory>
//{
//    private readonly HttpClient _client;

//    public ProductServiceIntegrationTest(CustomWebApplicationFactory factory)
//    {
//        _client = factory.CreateClient();
//    }

//    [Theory]
//    [InlineData("New Product 1", 10.00, 5)]
//    [InlineData("New Product 2", 20.00, 10)]
//    public async Task AddProduct_ShouldReflectInProductList(string name, double price, int quantity)
//    {
//        // Arrange
//        var newProductData = new StringContent(
//            JsonSerializer.Serialize(new { Name = name, Price = price.ToString(), Stock = quantity.ToString() }),
//            Encoding.UTF8,
//            "application/json");

//        // Act - Envoi de la requête POST pour ajouter un nouveau produit
//        var postResponse = await _client.PostAsync("/admin/addProduct", newProductData);
//        postResponse.EnsureSuccessStatusCode();

//        // Act - Récupération de la liste des produits
//        var getResponse = await _client.GetAsync("/products");
//        getResponse.EnsureSuccessStatusCode();

//        var productList = await JsonSerializer.DeserializeAsync<List<ProductViewModel>>(await getResponse.Content.ReadAsStreamAsync()) ?? new List<ProductViewModel>();

//        // Assert - Vérifier que le nouveau produit est ajouté
//        Assert.Contains(productList, p => p.Name == name &&
//                                          decimal.Parse(p.Price) == (decimal)price &&
//                                          int.Parse(p.Stock) == quantity);
//    }

//    [Fact]
//    public async Task DeleteProduct_ShouldRemoveProductFromList()
//    {
//        // Arrange - Supposez que nous avons un produit avec l'ID 1 que nous voulons supprimer
//        int productIdToDelete = 1;

//        // Act - Envoi de la requête DELETE pour supprimer le produit
//        var deleteResponse = await _client.DeleteAsync($"/admin/deleteProduct/{productIdToDelete}");
//        deleteResponse.EnsureSuccessStatusCode();

//        // Act - Récupération de la liste des produits pour vérifier la suppression
//        var getResponse = await _client.GetAsync("/products");
//        getResponse.EnsureSuccessStatusCode();
//        var productList = await JsonSerializer.DeserializeAsync<List<ProductViewModel>>(await getResponse.Content.ReadAsStreamAsync()) ?? new List<ProductViewModel>();

//        // Assert - Vérifier que le produit supprimé n'est plus présent
//        Assert.DoesNotContain(productList, p => p.Id == productIdToDelete);
//    }

//    [Theory]
//    [InlineData(1, "Updated Product", 15.00, 8)]
//    public async Task UpdateProduct_ShouldReflectUpdatedProductDetails(int productIdToUpdate, string newName, double newPrice, int newQuantity)
//    {
//        // Arrange - Préparation des données mises à jour pour le produit
//        var updatedProductData = new StringContent(
//            JsonSerializer.Serialize(new { Id = productIdToUpdate, Name = newName, Price = newPrice.ToString(), Stock = newQuantity.ToString() }),
//            Encoding.UTF8,
//            "application/json");

//        // Act - Envoi de la requête PUT/PATCH pour mettre à jour le produit
//        var updateResponse = await _client.PutAsync($"/admin/updateProduct/{productIdToUpdate}", updatedProductData); // ou PatchAsync selon votre API
//        updateResponse.EnsureSuccessStatusCode();

//        // Act - Récupération de la liste des produits pour vérifier la mise à jour
//        var getResponse = await _client.GetAsync("/products");
//        getResponse.EnsureSuccessStatusCode();
//        var productList = await JsonSerializer.DeserializeAsync<List<ProductViewModel>>(await getResponse.Content.ReadAsStreamAsync()) ?? new List<ProductViewModel>();

//        // Assert - Vérifier que les détails du produit sont mis à jour
//        Assert.Contains(productList, p => p.Id == productIdToUpdate && p.Name == newName && decimal.Parse(p.Price) == (decimal)newPrice && int.Parse(p.Stock) == newQuantity);
//    }

//}







//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Text;
//using System.Text.Json;


//public class ProductServiceIntegrationTest : IClassFixture<Program>
//{
//    private readonly WebApplicationFactory<Program> _factory;

//    public ProductServiceIntegrationTest(CustomWebApplicationFactory factory)
//    {
//        _factory = factory;
//    }

//    [Theory]
//    [InlineData("New Product 1", 10.00, 5)]
//    [InlineData("New Product 2", 20.00, 10)]
//    public async Task AddProduct_ShouldReflectInProductList(string name, double price, int quantity)
//    {
//        // Arrange - Préparation de la requête d'ajout d'un nouveau produit
//        var newProductData = new StringContent(
//            JsonSerializer.Serialize(new { Name = name, Price = price, Quantity = quantity }),
//            Encoding.UTF8,
//            "application/json");

//        // Act - Envoi de la requête d'ajout de produit
//        await _factory.PostAsync("/admin/addProduct", newProductData);

//        // Act - Récupération de la liste des produits pour vérifier l'ajout
//        var response = await _factory.GetAsync("/products");
//        response.EnsureSuccessStatusCode();

//        // Assert - Vérification que le nouveau produit est bien présent dans la liste des produits récupérée
//        var productList = await JsonSerializer.DeserializeAsync<List<P3AddNewFunctionalityDotNetCore.Models.ViewModels.ProductViewModel>>(await response.Content.ReadAsStreamAsync());
//        Assert.Contains(productList, p => p.Name == name && p.Price == price && p.Quantity == quantity);
//    }

//    // Vous pouvez étendre les tests pour inclure la suppression, la mise à jour, etc., en suivant la même structure.
//}