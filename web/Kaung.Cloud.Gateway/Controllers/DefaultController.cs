using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kaung.Cloud.Gateway.Controllers
{
    [Route("/")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return Content("gateway");
        }

        
        [HttpGet("/info")]
        public ActionResult<Object> Info()
        {
            var clientHost = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var serverHost = HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
            var path = HttpContext.Request.Path;
            return new 
            {
                RemoteIp = clientHost,
                RemotePort = HttpContext.Connection.RemotePort,
                LocalIp = serverHost,
                LocalPort = HttpContext.Connection.LocalPort,
                Path = path,
                Host = HttpContext.Request.Host.ToUriComponent()
            };
        }

        [HttpGet("/health")]
        public IActionResult Heathle()
        {
            return Ok();
        }

        [HttpPost("/notice")]
        public IActionResult Notice()
        {
            var bytes = new byte[10240];
            var i = Request.Body.ReadAsync(bytes, 0, bytes.Length);
            var content = System.Text.Encoding.UTF8.GetString(bytes).Trim('\0');
            
            //发送邮件 短信 等 todo

            return Ok();
        }

    }
}