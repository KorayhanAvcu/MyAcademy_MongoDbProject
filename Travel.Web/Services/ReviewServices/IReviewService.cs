using Travel.Web.DTOs.ReviewDtos;

namespace Travel.Web.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<List<ReviewListDto>> GetAllAsync();
        Task<ReviewDetailDto> GetByIdAsync(string id);
        Task CreateAsync(CreateReviewDto createReviewDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateReviewDto updateReviewDto);
        Task MarkAsReadAsync(string id);
        Task<List<ReviewListDto>> GetByTourAsync(string tourId);
        Task<List<ReviewListDto>> GetByUserAsync(string userId);
        Task<bool> HasUserReviewedAsync(string userId, string tourId);
    }
}