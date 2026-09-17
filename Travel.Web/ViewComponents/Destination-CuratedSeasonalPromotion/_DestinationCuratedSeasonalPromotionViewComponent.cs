using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.ViewComponents.Destination_CuratedSeasonalPromotion
{
    public class _DestinationCuratedSeasonalPromotionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
