using CSharpApp.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
    {
        var products = await productsService.GetProducts();
        return products;
    })
    .WithName("GetProducts")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (int id, IProductsService productsService) =>
{
    var products = await productsService.GetProductById(id);
    return products;
})
    .WithName("GetProduct")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/addproduct", async ([FromBody]AddProductRequest request, [FromServices]IProductsService productsService) =>
{
    var products = await productsService.AddProduct(request);
    return products; 
})
    .WithName("AddProduct")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPut("api/v{version:apiVersion}/updateproduct/{id:int}", async (int id, UpdateProductRequest request, [FromServices] IProductsService productsService) =>
{
    var products = await productsService.UpdateProduct(id, request);
    return products;
})
    .WithName("UpdateProduct")
    .HasApiVersion(1.0);



versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.GetCategories();
    return categories;
})
    .WithName("GetCategories")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategory/{id:int}", async (int id, ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.GetCategoryById(id);
    return categories;
})
    .WithName("GetCategory")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/addcategory", async (Category cat, ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.AddCategory(cat.Name!, cat.Image!);
    return categories;
})
    .WithName("AddCategory")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPut("api/v{version:apiVersion}/updatecategory/{id:int}", async (int id, Category cat, ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.UpdateCategory(id, cat.Name!, cat.Image!);
    return categories;
})
    .WithName("UpdateCategory")
    .HasApiVersion(1.0);

app.Run();