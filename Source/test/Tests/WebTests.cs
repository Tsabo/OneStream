using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OneStream.Api.Controllers;

namespace OneStream.Tests
{
    public class WebTests
    {
        [Fact]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging

            await using var app = await appHost.BuildAsync();
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("webfrontend");
            await resourceNotificationService.WaitForResourceAsync("webfrontend", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void GetWeatherVerifyTemperatureRange()
        {
            // Arrange
            var logger = new Logger<WeatherForecastController>(new NullLoggerFactory());
            var controller = new WeatherForecastController(logger);
            var range = Enumerable.Range(-20, 76);

            // Act
            var result = controller.Get();

            // Assert
            Assert.Collection(result,
                p => Assert.True(range.Contains(p.TemperatureC)),
                p => Assert.True(range.Contains(p.TemperatureC)),
                p => Assert.True(range.Contains(p.TemperatureC)),
                p => Assert.True(range.Contains(p.TemperatureC)),
                p => Assert.True(range.Contains(p.TemperatureC))
            );
        }
    }
}
