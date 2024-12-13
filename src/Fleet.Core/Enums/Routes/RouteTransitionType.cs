namespace Fleet.Core.Enums;

public enum RouteTransitionType
{
    /// <summary>
    /// Departure from a depot (first)
    /// </summary>
    Departure,

    /// <summary>
    /// Travel from one job to another
    /// </summary>
    Travel,

    /// <summary>
    /// Arrival at a depot (last)
    /// </summary>
    Arrival,
}