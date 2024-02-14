using inventory_system_aspdotnet_web_api.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<CustomerRepository>();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<OrderRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(x => x.AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithOrigins("http://localhost:4200", "http://192.168.0.76:3000"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
