
using Travel.Web.DTOs.QuestionDtos;

namespace Travel.Web.Services.QuestionServices
{
    public interface IQuestionService
    {
        Task<List<QuestionListDto>> GetAllAsync();
        Task<QuestionDetailDto> GetByIdAsync(string id);
        Task CreateAsync(CreateQuestionDto createQuestionDto);
        Task AnswerAsync(AnswerQuestionDto answerQuestionDto);
        Task MarkAsReadAsync(string id);
        Task DeleteAsync(string id);
        Task<int> GetUnreadCountAsync();
    }
}
