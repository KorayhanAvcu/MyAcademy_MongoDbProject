
namespace Travel.Web.DTOs.QuestionDtos
{
    public class CreateQuestionDto
    {
        public string? UserId { get; set; }

        public string TourId { get; set; } = default!;

        public string? UserName { get; set; }

        public string QuestionText { get; set; } = string.Empty;
    }
}

