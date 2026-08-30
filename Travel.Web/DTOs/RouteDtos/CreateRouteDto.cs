namespace Travel.Web.DTOs.RouteDtos
{
    public class CreateRouteDto
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Duration { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
