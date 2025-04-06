using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using DotNetFormApp.Models;
   using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;
public class FormController : Controller
{
    private readonly DataContextDapper _dataAccess;

    public FormController(DataContextDapper dataAccess)
    {
        _dataAccess = dataAccess;
    }

    // Display form
    public IActionResult Index()
    {
        return View();
    }

    // Submit form
    [HttpPost]
    public async Task<IActionResult> SubmitForm(FormData formData, IFormFile image)
    {
        if (image != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                formData.Image = memoryStream.ToArray();
            }
        }

        int id = await _dataAccess.InsertFormData(formData);
        return RedirectToAction("Success", new { id = id });
    }

    // Success page
    public async Task<IActionResult> Success(int id)
    {
        var formData = await _dataAccess.GetFormDataById(id);
        if (formData == null)
        {
            return NotFound();
        }

        return View(formData);
    }



public IActionResult GeneratePdf(int id)
{
    var formData = _dataAccess.GetFormDataById(id).Result;
    if (formData == null)
    {
        return NotFound();
    }

    var ms = new MemoryStream();
    var pdfDocument = new PdfDocument();
    var page = pdfDocument.AddPage();
    var gfx = XGraphics.FromPdfPage(page);
    var font = new XFont("Verdana", 12);

    // Add text to PDF
    gfx.DrawString($"Name: {formData.Name}", font, XBrushes.Black, 20, 50);
    gfx.DrawString($"Email: {formData.Email}", font, XBrushes.Black, 20, 80);
    gfx.DrawString($"Address: {formData.Address}", font, XBrushes.Black, 20, 110);

    // Add image to PDF if it exists
    if (formData.Image != null)
    {
        using (var imageStream = new MemoryStream(formData.Image))
        {
            XImage image = XImage.FromStream(() => imageStream);  // This accepts a Stream, which is MemoryStream
            gfx.DrawImage(image, 20, 140, 100, 100); // adjust position and size as needed
        }
    }

    // Save the PDF to the memory stream
    pdfDocument.Save(ms);
    ms.Seek(0, SeekOrigin.Begin);

    return File(ms.ToArray(), "application/pdf", "formdata.pdf");
}}
 // Generate PDF
       
