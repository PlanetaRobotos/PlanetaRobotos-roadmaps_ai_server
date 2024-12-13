using Fleet.Application.Models.Depots;
using Fleet.Application.Models.Jobs;
using Fleet.Application.Models.Routes;
using Fleet.Application.Models.Shared;
using Fleet.Application.Models.Vehicles;
using Fleet.Core.Enums;
using Fleet.Infrastructure.Services;

var service = new RouteOptimizationService();
var startTime = DateTime.Today.AddHours(24 + 8);

var request = new RouteOptimizationEnrichedModel
{
    StartTime = startTime,
    ConsiderRoadTraffic = true,
    Jobs =
    [
        new JobModel
        {
            Type = JobType.Collection,
            Location = new LocationModel(latitude: 50.473735740149145, longitude: 30.448875732776752),
            TimeWindowFrom = startTime,
            TimeWindowTo = startTime.AddHours(1)
        },
        new RouteOptimizationNode
        {
            Type = JobType.Delivery,
            Location = new LocationModel(latitude: 50.463814829655306, longitude: 30.522671069903353),
        },
        new RouteOptimizationNode
        {
            Type = JobType.Collection,
            Location = new LocationModel(latitude: 50.46970791636328, longitude: 30.521200008787474),
        },
        new RouteOptimizationNode
        {
            Type = JobType.Delivery,
            Location = new LocationModel(latitude: 50.47716583818333, longitude: 30.533061837443732),
        },
        new RouteOptimizationNode
        {
            Type = JobType.Collection,
            Location = new LocationModel(latitude: 50.458558183597944, longitude: 30.61396394539339),
        },
        new RouteOptimizationNode
        {
            Type = JobType.Delivery,
            Location = new LocationModel(latitude: 50.49030213848951, longitude: 30.486810859874257),
        },
        new RouteOptimizationNode
        {
            Type = JobType.Collection,
            Location = new LocationModel(latitude: 50.523216219941595, longitude: 30.4991448127208),
            TimeWindowFrom = startTime.AddMinutes(150),
            TimeWindowTo = startTime.AddMinutes(190)
        },
    ],
    Vehicles =
    [
        new VehicleModel
        {
            Id = Guid.NewGuid(),
            Name = "White Volvo",
            Type = VehicleType.Track,
            StartDepot = new DepotModel
            {
                Location = new LocationModel(latitude: 50.443266962211965, longitude: 30.624063785109417)
            },
            EndDepot = new DepotModel
            {
                Location = new LocationModel(latitude: 50.42030467878708, longitude: 30.431674824367178)
            }
        },
        new VehicleModel
        {
            Id = Guid.NewGuid(),
            Name = "Black Wolkswagen Sprinter",
            Type = VehicleType.Track,
            StartDepot = new DepotModel
            {
                Location = new LocationModel(latitude: 50.42030467878708, longitude: 30.431674824367178)
            },
            EndDepot = new DepotModel
            {
                Location = new LocationModel(latitude: 50.443266962211965, longitude: 30.624063785109417)
            }
        }
    ]
};

service.OptimizeRoutesAsync(request).Wait();