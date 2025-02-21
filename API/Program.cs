using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Services;
using SalesDatePrediction.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilitar CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Permitir Angular
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Registrar servicios
builder.Services.AddScoped<ISalesDatePredictionService, SalesDatePredictionService>();
builder.Services.AddScoped<ISalesDatePredictionRepository, SalesDatePredictionRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IShipperService, ShipperService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Registrar repositorios
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar CORS antes de Authorization
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();

app.Run();
