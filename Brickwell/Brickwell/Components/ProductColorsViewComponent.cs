using Microsoft.AspNetCore.Mvc;
using Brickwell.Models;

namespace Brickwell.Components;

public class ProductColorsViewComponent : ViewComponent
{
    private IBrickwellRepository _repo;
    
    public ProductColorsViewComponent(IBrickwellRepository temp)
    {
        _repo = temp;
    }

    public IViewComponentResult Invoke()
    {
        var productColors = _repo.Products
            .Select(x => x.primary_color)
            .Distinct()
            .OrderBy(x => x);

        return View(productColors);
    }
}