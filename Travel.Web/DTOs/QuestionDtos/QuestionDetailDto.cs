namespace Travel.Web.DTOs.QuestionDtos
{
    public class QuestionDetailDto
    {
        public string Id { get; set; } = default!;

        public string UserId { get; set; } = default!;

        public string TourId { get; set; } = default!;

        public string TourName { get; set; } = string.Empty;

        public string QuestionText { get; set; } = string.Empty;

        public string? Answer { get; set; }

        public DateTime? AnsweredAt { get; set; }

        public bool IsAnswered { get; set; }

        public bool IsRead { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}

