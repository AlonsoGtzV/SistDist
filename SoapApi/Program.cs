using Microsoft.EntityFrameworkCore;
using SoapApi.Contracts;
using SoapApi.Infrastructure;
using SoapApi.Repositories;
using SoapApi.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
<<<<<<< HEAD

// Registrar servicios
builder.Services.AddSoapCore();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserContract, UserService>();
builder.Services.AddScoped<IBookContract, BookService>();
=======
builder.Services.AddSoapCore();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserContract, UserService>();


>>>>>>> main
builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
<<<<<<< HEAD

// Configurar ambos endpoints SOAP
app.UseSoapEndpoint<IUserContract>("/UserService.svc", new SoapEncoderOptions());
app.UseSoapEndpoint<IBookContract>("/BookService.svc", new SoapEncoderOptions());

// Ejecutar la aplicación
app.Run();
=======
app.UseSoapEndpoint<IUserContract>("/UserService.svc", new SoapEncoderOptions());

app.Run();
>>>>>>> main
