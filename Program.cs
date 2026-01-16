using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.RealIpHeader = "X-Forwarded-For";
    options.ClientIdHeader = "X-ClientId";
    options.HttpStatusCode = StatusCodes.Status429TooManyRequests;
    options.GeneralRules =
    [
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 60
        }
    ];
});
builder.Services.Configure<IpRateLimitPolicies>(options =>
{
    options.IpRules =
    [
        new IpRateLimitPolicy
        {
            Ip = "*",
            Rules =
            [
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1m",
                    Limit = 60
                }
            ]
        }
    ];
});
builder.Services.Configure<ClientRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = StatusCodes.Status429TooManyRequests;
    options.GeneralRules =
    [
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 120
        }
    ];
});
builder.Services.Configure<ClientRateLimitPolicies>(options =>
{
    options.ClientRules =
    [
        new ClientRateLimitPolicy
        {
            ClientId = "*",
            Rules =
            [
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1m",
                    Limit = 120
                }
            ]
        }
    ];
});
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    KnownNetworks = { },
    KnownProxies = { }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseIpRateLimiting();
app.UseClientRateLimiting();

app.MapControllers();

app.Run();
