using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Tour_Questions
{
    public class _TourQuestionsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
    }
}
