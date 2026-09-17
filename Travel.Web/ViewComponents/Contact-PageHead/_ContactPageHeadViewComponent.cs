using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Contact_PageHead
{
    public class _ContactPageHeadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }   
    }
}
