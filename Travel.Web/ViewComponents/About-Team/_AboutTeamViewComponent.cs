using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.AboutTeamMemberServices;

namespace Travel.Web.ViewComponents.About_Team
{
    public class _AboutTeamViewComponent(
        IAboutTeamMemberService aboutTeamMemberService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await aboutTeamMemberService.GetAllAsync();

            return View(values);
        }
    }
}