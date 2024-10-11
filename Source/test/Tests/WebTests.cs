using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OneStream.Api.Controllers;
using OneStream.Api.Services.Abstractions;
using OneStream.Api.DataObjects;

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

        [Fact]
        public async Task DeletePerson()
        {
            // Arrange
            var id = Guid.NewGuid();

            var logger = new Logger<PeopleController>(new NullLoggerFactory());

            var repository = new Mock<IPeopleRepo>();
            repository
                .Setup(p => p.DeletePersonAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            
            var controller = new PeopleController(logger, repository.Object);

            // Act
            var result = await controller.DeletePerson(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            if (result is not OkObjectResult { Value: bool deleted })
                return;

            Assert.True(deleted);
        }

        [Fact]
        public async Task UpdatePerson()
        {
            // Arrange
            var id = Guid.NewGuid();

            var logger = new Logger<PeopleController>(new NullLoggerFactory());

            var repository = new Mock<IPeopleRepo>();
            repository
                .Setup(p => p.UpdatePersonAsync(It.IsAny<Guid>(), It.IsAny<EditPersonDto>()))
                .ReturnsAsync(true);

            var controller = new PeopleController(logger, repository.Object);

            var person = new EditPersonDto { Name = "Test 2", Email = "test2@test.com" };

            // Act
            var result = await controller.UpdatePerson(id, person);
            
            // Assert
            repository.Verify(p => p.UpdatePersonAsync(id, person));

            Assert.IsType<OkObjectResult>(result);

            if (result is not OkObjectResult { Value: bool updated })
                return;

            Assert.True(updated);
        }

        [Fact]
        public async Task GetPersons()
        {
            // Arrange
            var logger = new Logger<PeopleController>(new NullLoggerFactory());

            var repository = new Mock<IPeopleRepo>();
            repository
                .Setup(p => p.GetPeopleAsync())
                .ReturnsAsync([
                    new PersonDto { Id = Guid.NewGuid(), Name = "Test 1", Email = "test1@test.com" },
                    new PersonDto { Id = Guid.NewGuid(), Name = "Test 2", Email = "test2@test.com" }
                ]);

            var controller = new PeopleController(logger, repository.Object);

            // Act
            var result = await controller.GetPeople();

            // Assert

            repository.Verify(p => p.GetPeopleAsync());

            Assert.IsType<OkObjectResult>(result);

            if (result is not OkObjectResult { Value: PersonDto[] persons })
                return;
            
            Assert.Collection(persons,
                p =>
                {
                    Assert.IsType<PersonDto>(p);
                    Assert.Equal("Test 1", p.Name);
                },
                p =>
                {
                    Assert.IsType<PersonDto>(p);
                    Assert.Equal("Test 2", p.Name);
                }
            );
        }
    }
}
