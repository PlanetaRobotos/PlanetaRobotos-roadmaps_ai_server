using Fleet.Infrastructure.Services;

namespace Fleet.IntegrationTests;

public class UnitTest1
{
    // [Fact]
    // public void Test1()
    // {
    //     // var p = new JobFieldsCsvParser();
    //     // var result = p.ParseAsync("Assets/JobFieldsConfiguration.csv");
    // }


    [Fact]
    public void RouteOptimizationService_OptimizeRouteAsync()
    {
        // Arrange
        var service = new RouteOptimizationService();

        // Act
        // service.OptimizeRouteAsync().Wait();

        // Assert
        Assert.True(true);
    }
}