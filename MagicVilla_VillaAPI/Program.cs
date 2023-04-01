using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;



//In summary, the WebApplication.CreateBuilder(args) method is used to create a new instance of the WebApplicationBuilder class, 
//which is used to configure the application's services, middleware pipeline, and other settings that are necessary for the 
//application to function properly.
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddScoped<IVillaRepository,VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the web host.
//In the context of .NET web applications, a web host is a process that listens 
//for incoming HTTP requests and processes them by invoking the appropriate parts of the application code
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//aUseHttpsRedirection() method is a middleware component in ASP.NET Core
//When a user makes an HTTP request to the application, the UseHttpsRedirection() middleware 
//intercepts the request and checks if it is secure. If the request is not secure, the middleware generates 
//a redirect response to the HTTPS version of the URL and sends it back to the client. The client then makes 
//a new request to the HTTPS URL, and the request is processed by the application

app.UseHttpsRedirection();


//The UseAuthorization() method is a middleware component in ASP.NET Core
//When a request is received, the UseAuthorization() middleware checks if the request contains an authentication token
//or cookie. If the request is not authenticated, the middleware returns a 401 Unauthorized response. If the request is
//authenticated, the middleware checks if the user has the necessary permissions to access the requested resource. If the
//user is not authorized, the middleware returns a 403 Forbidden response.
app.UseAuthorization();


//---------------------------------
//The MapControllers() method is a middleware component in ASP.NET Core that maps the application's endpoints to the appropriate
//action methods in the controller classes. It is used to route incoming requests to the appropriate controller and action method
//based on the URL and HTTP method.

//When a request is received, the MapControllers() middleware component looks at the incoming request's URL and HTTP method to determine
//the appropriate controller and action method to handle the request. It then invokes the appropriate action method and returns the response
//to the client
app.MapControllers();


//-----------------------------------
// The app.Run() method is a terminal middleware component in ASP.NET Core that is used to handle the final processing of an HTTP request and
// generate a response to send back to the client. It is typically used to handle any remaining requests that have not been handled by other
// middleware components in the application's request pipeline.
app.Run();
