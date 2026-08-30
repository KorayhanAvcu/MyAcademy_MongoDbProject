namespace Travel.Web.DTOs.BannerDtos
{
    public class UpdateBannerDto
    {
        public string Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
