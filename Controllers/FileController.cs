using CRUD_The_First.Data;
using CRUD_The_First.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_The_First.Controllers;

public class FileController : Controller
{
    private readonly ApplicationContext _context;
    public FileController(ApplicationContext context)
    {
        this._context = context;
    }
    // GET
    public IActionResult Index()
    {
        IEnumerable<FIleModel> objFileList = _context.Files;
        return View(objFileList);
    }
}
