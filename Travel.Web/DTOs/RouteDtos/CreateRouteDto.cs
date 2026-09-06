namespace Travel.Web.DTOs.RouteDtos
{
    public class CreateRouteDto
    {
        public string DestinationId { get; set; } = string.Empty;
        public string? Duration { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
