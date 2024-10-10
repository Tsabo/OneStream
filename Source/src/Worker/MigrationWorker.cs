using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OneStream.Api.Data;
using OneStream.Backend.Data;
using OpenTelemetry.Trace;

namespace OneStream.Worker
{
    public class MigrationWorker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";
        private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);
            using var scope = serviceProvider.CreateScope();
            await MigrateDatabase<IdentityDbContext>(scope, activity, cancellationToken);
            await MigrateDatabase<ApplicationDbContext>(scope, activity, cancellationToken);

            hostApplicationLifetime.StopApplication();
        }

        private static async Task MigrateDatabase<T>(IServiceScope scope, Activity activity, CancellationToken cancellationToken = default) where T : DbContext
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<T>();
                await EnsureDatabaseAsync(context, cancellationToken);
                await RunMigrationAsync(context, cancellationToken);
            }
            catch (Exception ex)
            {
                activity?.RecordException(ex);
            }
        }

        private static async Task EnsureDatabaseAsync<T>(T dbContext, CancellationToken cancellationToken) where T: DbContext
        {
            var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Create the database if it does not exist.
                // Do this first so there is then a database to start a transaction against.
                if (!await dbCreator.ExistsAsync(cancellationToken))
                    await dbCreator.CreateAsync(cancellationToken);
            });
        }

        private static async Task RunMigrationAsync<T>(T dbContext, CancellationToken cancellationToken) where T : DbContext
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                await dbContext.Database.MigrateAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });
        }
    }
}
