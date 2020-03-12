using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ExportPDFKendoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Save(string contentType, string base64, string fileName)
//{
//    var fileContents = Convert.FromBase64String(base64);

//    return File(fileContents, contentType, fileName);
//}