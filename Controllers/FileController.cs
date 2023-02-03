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
        IEnumerable<FileModel> objFileList = _context.Files;
        return View(objFileList);
    }
    //GET
    public IActionResult Create()
    {
        return View();
    }
}
