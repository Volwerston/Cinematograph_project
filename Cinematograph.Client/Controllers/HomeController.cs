using System.Diagnostics;
using Application;
using Microsoft.AspNetCore.Mvc;
using Cinematograph.Client.Models;

namespace Cinematograph.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly MessageProcessor _processor = new MessageProcessor();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetUsername(string userName)
        {
            _processor.Username = userName ?? string.Empty;
            return Ok();
        }

        [HttpGet]
        public IActionResult GetResponse(string request)
            => Ok(new
            {
                Message = _processor.ProcessMessage(request)
            });

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
