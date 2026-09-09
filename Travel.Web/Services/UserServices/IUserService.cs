using Travel.Web.DTOs.UserDtos;

namespace Travel.Web.Services.UserServices
{
    public interface IUserService
    {
        Task<List<ResultUserDto>> GetAllAsync();

        Task<UserDetailDto> GetByIdAsync(string id);

        Task<bool> CreateAsync(CreateUserDto dto);

        Task<bool> UpdateAsync(UpdateUserDto dto);

        Task<bool> DeleteAsync(string id);
    }
}