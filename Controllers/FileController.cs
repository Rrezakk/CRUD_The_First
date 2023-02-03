using Microsoft.AspNetCore.Mvc;

namespace CRUD_The_First.Controllers;

public class FileController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
