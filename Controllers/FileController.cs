using CRUD_The_First.Data;
using CRUD_The_First.Models;
using CRUD_The_First.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    //GET
    public async Task<IActionResult> Edit(int? id)
    {
        if (id==null)
        {
            return NotFound();
        }
        var fileFromDb = await _context.Files.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
        if (fileFromDb == null)
        {
            return NotFound();
        }

        return View(fileFromDb);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async  Task<ActionResult> Create(CreationFileModel model)
    {
        if (model.FileName.Length>100)
        {
            ModelState.AddModelError("FileName","Must be less than 100");
        }
        if (!ModelState.IsValid)
        {
            Debug.WriteLine($"Invalid model!");
            return View(model);
        }
        Debug.WriteLine($"{model.FileName} {model.CreationTime} {model.StaticUrl}");
        try
        {
            var tup = await _bufferedFileUploadService.UploadFile(model.File);
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
            Debug.WriteLine($"File Upload Failed: {ex}");
            ViewBag.Message = "File Upload Failed";
            //Logging exception
        }
        
        return RedirectToAction("Index");
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async  Task<ActionResult> Edit(FileModel model)
    {
        var fileFromDb = await _context.Files.FindAsync(model.Id);
        if (string.IsNullOrEmpty(model.FileName)||model.FileName.Length>100)
        {
            ModelState.AddModelError("FileName","Must be non-empty and less than 100");
        }
        else
        {
            if (fileFromDb == null) return View(model);
            fileFromDb.FileName = model.FileName; 
            _context.Files.Update(fileFromDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(model);
    }
    //GET
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var fileFromDb = await _context.Files.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
        if (fileFromDb == null)
            return NotFound();
        return View(fileFromDb);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int? id)
    {
        if (id != null)
        {
            var fileFromDb = await _context.Files.FindAsync(id);
            if (fileFromDb == null)
                return NotFound();
            var path = Environment.CurrentDirectory + fileFromDb.StaticUrl;
            _context.Files.Remove(fileFromDb);
            await _context.SaveChangesAsync();
            await Task.Run(() =>
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                    //ignored
                }
                
            });
        }
        return RedirectToAction("Index");
    }
    //GET
    public async Task<IActionResult> Download(int? id)
    {
        if (id != null)
        {
            var fileFromDb = await _context.Files.FindAsync(id);
            if (fileFromDb == null)
                return NotFound();
            string baseUrl = Request.GetDisplayUrl();
            var index = baseUrl.IndexOf("File");
            baseUrl = baseUrl[0..index];
            var url = baseUrl + fileFromDb.StaticUrl.TrimStart('\\').Replace("\\", "/");
            return RedirectToRoute(url);
        }
        return RedirectToAction("Index");
    }
}
