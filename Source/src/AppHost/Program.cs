var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Backend>("backend");

builder.AddProject<Projects.Api>("api")
    .WithExternalHttpEndpoints();

builder.Build().Run();
