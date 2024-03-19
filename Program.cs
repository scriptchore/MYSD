using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MYSD_REG.Api.Extentions;
using MYSD_REG.Core.Models;
using MYSD_REG.Data;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog(((ctx, lc) => lc

.ReadFrom.Configuration(ctx.Configuration)));

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors((options) =>
{
    options.AddPolicy("MYSD.API", (corsPolicy) =>
    {
        corsPolicy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
//});
builder.Services.ConfigureDepencyInjections();


builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MYSD_CONN"), providerOptions => { providerOptions.EnableRetryOnFailure(); }));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Configure identity options here
})
.AddEntityFrameworkStores<DataContext>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("MYSD.API");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
