using Microsoft.AspNetCore.Mvc;

namespace Brickwell.Components
{
    public class PageSizeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
