using AwsCognito.Test.API;
using AwsCognito.Test.API.Authentication;
using AwsCognito.Test.API.Exceptions;
using AwsCognito.Test.Application;
using AwsCognito.Test.Domain;
using AwsCognito.Test.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddApplicationModule()
    .AddDomainModule()
    .AddInfrastructureModule(configuration);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCognitoAuthentication(configuration);
builder.Services.AddFrontendCors(configuration);

var app = builder.Build();

app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
