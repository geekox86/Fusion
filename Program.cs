var appBuilder = WebApplication.CreateBuilder(args);

appBuilder.Services.AddGraphQLServer("X").AddQueryType<QueryX>(c => c.Name("Query"));
appBuilder.Services.AddGraphQLServer("Y").AddQueryType<QueryY>(c => c.Name("Query"));

appBuilder
    .Services
    .AddHttpClient("X", c => c.BaseAddress = new Uri("http://localhost:5056/graphql/x"));
appBuilder
    .Services
    .AddHttpClient("Y", c => c.BaseAddress = new Uri("http://localhost:5056/graphql/y"));

appBuilder
    .Services
    .AddFusionGatewayServer("Fusion")
    .ConfigureFromFile("fusion.fgp")
    .ModifyFusionOptions(o => o.IncludeDebugInfo = true)
    .ModifyRequestOptions(o => o.IncludeExceptionDetails = true);

var app = appBuilder.Build();

app.MapGraphQL("/graphql/x", "X");
app.MapGraphQL("/graphql/y", "Y");
app.MapGraphQL("/graphql", "Fusion");
app.Run();

public sealed class QueryX
{
    public string GetHello() => "Hello!";
}

public sealed class QueryY
{
    public string GetWorld() => "World!";
}
