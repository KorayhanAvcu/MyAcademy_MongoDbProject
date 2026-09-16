using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.About_Team
{
    public class _AboutTeamViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
