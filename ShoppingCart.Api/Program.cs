

using FluentValidation.AspNetCore;

using LiteDB;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ShoppingCart.Api.Behaviors;
using ShoppingCart.Application;
using ShoppingCart.Infrastructure.Data;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<ShoppingCartContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("relacional"));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine);
    }
});

builder.Services.AddScoped<ILiteDatabase, LiteDatabase>(c => new LiteDatabase("UserBaskets.db"));
builder.Services.AddScoped<UserBasketContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserBasketRepository, DocumentUserBasketRepository>();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(typeof(Response).Assembly);

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)); builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>)); builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


builder.Services.AddControllers()
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
