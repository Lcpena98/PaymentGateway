using Microsoft.EntityFrameworkCore;
using PaymentGatewayApi.Data;
using PaymentGatewayApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<Gateway1Service>(c =>
{
    c.BaseAddress = new Uri("http://localhost:3001");
});

builder.Services.AddHttpClient<Gateway2Service>(c =>
{
    c.BaseAddress = new Uri("http://localhost:3002");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Add services to the container.
builder.Services.AddScoped<PaymentService>();
//
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
