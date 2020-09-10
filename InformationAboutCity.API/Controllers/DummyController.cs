using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformationAboutCity.API.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace InformationAboutCity.API.Controllers
{
                                  //this controller is used to create the database. by calling the TestDatabase the \\
                                  // constructor initializes the CityInfoContext and the database is generated \\

    public class DummyController : ControllerBase
    {
        private readonly CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        [HttpGet]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
