namespace AeonRegistryAPI.Models.Request;

public class CreateSiteRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
