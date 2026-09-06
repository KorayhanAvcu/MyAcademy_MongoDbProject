namespace Travel.Web.DTOs.DestinationDtos
{
    public class GetDestinationByIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
