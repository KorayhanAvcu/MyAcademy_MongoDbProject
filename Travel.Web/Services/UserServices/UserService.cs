using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Travel.Web.DTOs.UserDtos;
using Travel.Web.Entities;

namespace Travel.Web.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<ResultUserDto>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();

            return _mapper.Map<List<ResultUserDto>>(users);
        }

        public async Task<UserDetailDto> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            return _mapper.Map<UserDetailDto>(user);
        }

        public async Task<bool> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<AppUser>(dto);

            user.TermsAcceptedAt = dto.TermsAccepted
                ? DateTime.UtcNow
                : null;

            var result = await _userManager.CreateAsync(
                user,
                dto.Password
            );

            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);

            if (user == null)
                return false;

            _mapper.Map(dto, user);

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}