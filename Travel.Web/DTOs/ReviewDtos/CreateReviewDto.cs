namespace Travel.Web.DTOs.ReviewDtos
{
    public class CreateReviewDto
    {
        public string TourId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? UserImageUrl { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
