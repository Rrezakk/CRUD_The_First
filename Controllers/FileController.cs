using CRUD_The_First.Data;
using CRUD_The_First.Models;
using CRUD_The_First.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CRUD_The_First.Controllers;

public class FileController : Controller
{
    private readonly ApplicationContext _context;
    private readonly IBufferedFileUploadService _bufferedFileUploadService;
    public FileController(ApplicationContext context, IBufferedFileUploadService bufferedFileUploadService)
    {
        this._context = context;
        _bufferedFileUploadService = bufferedFileUploadService;
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
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async  Task<ActionResult> Create(FileModel model,IFormFile upload)
    {
        Debug.WriteLine($"{model.FileName} {model.CreationTime} {model.StaticUrl}");
        try
        {
            var tup = await _bufferedFileUploadService.UploadFile(upload);
            if (tup.Item1)
            {
                model.StaticUrl = tup.Item2;
                Debug.WriteLine($"{model.FileName} {model.CreationTime} {model.StaticUrl}");

                _context.Files.Add(model);
                await _context.SaveChangesAsync();
                Debug.WriteLine("File Upload Successful");
                ViewBag.Message = "File Upload Successful";
            }
            else
            {
                Debug.WriteLine("File Upload Failed");
                ViewBag.Message = "File Upload Failed";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("File Upload Failed");
            ViewBag.Message = "File Upload Failed";
            //Logging exception
        }
        
        return RedirectToAction("Index");
    }
}
