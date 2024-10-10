using OneStream.Api.Data;
using OneStream.Backend.Data;

namespace OneStream.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.AddServiceDefaults();
            builder.Services.AddHostedService<MigrationWorker>();

            builder.Services.AddOpenTelemetry()
                .WithTracing(tracing => tracing.AddSource(MigrationWorker.ActivitySourceName));

            builder.AddSqlServerDbContext<IdentityDbContext>("OneStream");
            builder.AddSqlServerDbContext<ApplicationDbContext>("OneStream");

            var host = builder.Build();
            host.Run();
        }
    }
}
