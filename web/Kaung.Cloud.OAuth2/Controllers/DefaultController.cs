﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kaung.Cloud.Gateway.Controllers
{
    [Route("/")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return Content("oauth2");
        }
    }
}