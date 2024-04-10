using Brickwell.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brickwell.Components;

public class ProductTypesViewComponent : ViewComponent
{
    private IBrickwellRepository _repo;
    public ProductTypesViewComponent(IBrickwellRepository temp)
    {
        _repo = temp;
    }
    
    public IViewComponentResult Invoke()
    {
        var productCategories = _repo.Products
            .Select(x => x.category)
            .Distinct()
            .OrderBy(x => x);

        return View(productCategories);
    }
}