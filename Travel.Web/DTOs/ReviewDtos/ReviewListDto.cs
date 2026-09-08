namespace Travel.Web.DTOs.ReviewDtos
{
    public class ReviewListDto
    {
        public string Id { get; set; }
        public string TourId { get; set; }
        public string TourName { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? UserImageUrl { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }

        public bool IsRead { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
