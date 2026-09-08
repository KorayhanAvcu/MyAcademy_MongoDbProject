
namespace Travel.Web.DTOs.QuestionDtos
{
    public class CreateQuestionDto
    {
        public string UserId { get; set; } = default!;

        public string TourId { get; set; } = default!;

        public string QuestionText { get; set; } = string.Empty;
    }
}

