// using System.Diagnostics.CodeAnalysis;
// using CourseAI.Application.Core;
// using CourseAI.Domain.Entities.Abstractions;
// using Mapster;
//
// namespace CourseAI.Application.Models.Shared;
//
// public class CoordinatesModel : ICustomMappable<CoordinatesModel, Coordinates>
// {
//     public CoordinatesModel() { }
//
//     [SetsRequiredMembers]
//     public CoordinatesModel(double latitude, double longitude)
//     {
//         Latitude = latitude;
//         Longitude = longitude;
//     }
//
//     /// <example>38.52449142300208</example>
//     public required double Latitude { get; init; }
//
//     /// <example>-8.893377553729714</example>
//     public required double Longitude { get; init; }
//
//
//     public Coordinates ToEntity()
//     {
//         return new Coordinates(Longitude, Latitude);
//     }
//
//     public static CoordinatesModel FromEntity(Coordinates coordinates)
//     {
//         return new CoordinatesModel
//         {
//             Latitude = coordinates.Latitude,
//             Longitude = coordinates.Longitude
//         };
//     }
//
//     public void ConfigureMapper(TypeAdapterSetter<CoordinatesModel, Coordinates> config)
//     {
//         config
//             .ConstructUsing(src => new Coordinates(src.Longitude, src.Latitude));
//     }
//
//     public void ConfigureInvertMapper(TypeAdapterSetter<Coordinates, CoordinatesModel> config)
//     {
//         config
//             .Map(dest => dest.Longitude, src => src.Longitude)
//             .Map(dest => dest.Latitude, src => src.Latitude);
//     }
// }