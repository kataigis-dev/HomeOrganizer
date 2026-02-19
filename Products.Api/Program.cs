using Helpers.Common.Models;
using Helpers.Web;
using Helpers.Web.Filters;
using Microsoft.EntityFrameworkCore;
using Products.Api.Infrastructure;
using Products.Application.Filters;
using Products.Application.Models;
using Products.Application.Services;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Database
builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProductsDb")));

// Configure DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITransactionHandler, TransactionHandler>();
builder.Services.AddScoped<Helpers.Web.Controllers.IApplicationService<ProductFilters, ProductDetailedDto, ProductDetailedDto, ProductDto>, ProductsService>();

// Configure Translation
Helpers.Web.DIContainerExtensions.RegisterTranslation(builder.Services, builder.Configuration);

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
