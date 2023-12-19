using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Syncfusion.DocIO.DLS;
using WebGrease.Activities;
using TestMVCApp.Models;

namespace TestMVCApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DemoRestrictEdit(string restrictType)
        {
            //Convert to stream for later use
            Stream stream = new MemoryStream();
            WordDocument doc = new WordDocument("C:\\Temp\\testRestrictEdit.docx");
            doc.Save(stream, Syncfusion.DocIO.FormatType.Docx);

            stream.Position = 0;

            Syncfusion.EJ2.DocumentEditor.WordDocument document = Syncfusion.EJ2.DocumentEditor.WordDocument.Load(stream, Syncfusion.EJ2.DocumentEditor.FormatType.Docx);
            Models.PageModel model = new Models.PageModel();
            model.sfdtContent = JsonConvert.SerializeObject(document);
            model.RestrictType = restrictType;
            document.Dispose();

            return View(model);
        }

        [HttpPost]
        public ActionResult DemoDocSave(Models.PageModel model)
        {
            //SFDT -> Word Stream
            var document = Syncfusion.EJ2.DocumentEditor.WordDocument.Save(model.sfdtContent, Syncfusion.EJ2.DocumentEditor.FormatType.Docx);

            //save the doc to memory stream
            var memStream = new MemoryStream();
            document.CopyTo(memStream);

            //save the memory stream to disk
            WordDocument doc = new WordDocument(memStream);
            doc.Save("C:\\Temp\\testRestrictEdit.docx");

            //Return with one property set to true
            return Json(new { success = true });
        }

        public ActionResult DemoTooBigFile()
        {
            //Convert to stream for later use
            Stream stream = new MemoryStream();
            WordDocument doc = new WordDocument("C:\\Temp\\test.docx");
            doc.Save(stream, Syncfusion.DocIO.FormatType.Docx);

            //doc.Save("C:\\Temp\\test2.docx");
            //doc.Dispose();


            //doc.Open("C:\\Temp\\test2.docx");
            //doc.Save(stream, Syncfusion.DocIO.FormatType.Docx);

            stream.Position = 0;

            Syncfusion.EJ2.DocumentEditor.WordDocument document = Syncfusion.EJ2.DocumentEditor.WordDocument.Load(stream, Syncfusion.EJ2.DocumentEditor.FormatType.Docx);
            Models.PageModel model = new Models.PageModel();
            model.sfdtContent = JsonConvert.SerializeObject(document);
            document.Dispose();

            return View(model);
        }

        [HttpPost]
        public JsonResult RestrictEditing(CustomRestrictParameter param)
        {
            if (param.passwordBase64 == "" && param.passwordBase64 == null)
            {
                return null;
            }
            else
            {
                var salt = param.saltBase64 ?? ""; //handle when received null
                var ret = Syncfusion.EJ2.DocumentEditor.WordDocument.ComputeHash(param.passwordBase64, salt, param.spinCount);
                return Json(ret);
            }
        }

    }
}