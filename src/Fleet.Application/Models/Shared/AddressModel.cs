namespace Fleet.Application.Models.Shared;

public class AddressModel
{
    public string Text { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
}