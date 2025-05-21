
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TicTacToe.Server;

var builder = WebApplication.CreateBuilder(args);

// Variables for MySQL connection
var Mysql_Server = "localhost";
var Mysql_User = "external_user";
var Mysql_Password = "password"; // very weak password for testing purposes
var Mysql_Database = "my_database";
var Mysql_Port = "3306"; // Default MySQL port
var Mysql_ConnectionString = $"server={Mysql_Server};port={Mysql_Port};user={Mysql_User};password={Mysql_Password};database={Mysql_Database}";

// Add MySQL database context
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(Mysql_ConnectionString, ServerVersion.AutoDetect(Mysql_ConnectionString)) // Use MySQL connection string
);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Some API v1", Version = "v1" });
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Redirect root URL to /swagger
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }
        await next();
    });
}

app.MapControllers();
app.Run();
