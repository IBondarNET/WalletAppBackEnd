using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Repositories;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Services;
using WalletApp.Backend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WalletContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(config.GetConnectionString("NpgConnection")));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAuthorizerUserRepository, AuthorizerUserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();

builder.Services.AddTransient<IDailyPointsService, DailyPointsService>();
builder.Services.AddTransient<IImageService, ImageService>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();