var builder = DistributedApplication.CreateBuilder(args);

// SQL
var sqlServer = builder
    .AddSqlServer("sql-server")
    .WithDataVolume();

var database = sqlServer.AddDatabase("OneStream");

builder.AddProject<Projects.Worker>("migrations")
    .WithReference(database);

builder.AddProject<Projects.Backend>("backend")
    .WithReference(database);

builder.AddProject<Projects.Api>("api")
    .WithReference(database)
    .WithExternalHttpEndpoints();

builder.Build().Run();
