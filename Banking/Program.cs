using Banking.Db;
using Banking.Db.Entities;
using Banking.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDb")));

//test sqlite db
var dbPath = $"{Directory.GetCurrentDirectory()}/db.sqlite";
builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TransactionService>();
var app = builder.Build();


using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetService<BankingDbContext>();

//seeding test data
if (!context.Accounts.Any())
{
    var accounts = new List<Account>
    {
        new Account { Id = 1, Balance = 10_000 },
        new Account { Id = 2, Balance = 5_000 },
        new Account { Id = 3, Balance = 3_500.25m },
        new Account { Id = 4, Balance = 2_000 },
        new Account { Id = 5, Balance = 450.50m }
    };

    context.Accounts.AddRange(accounts);
    context.SaveChanges();
}

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


