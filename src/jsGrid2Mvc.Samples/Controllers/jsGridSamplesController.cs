using jsGrid2Mvc.Configuration;
using jsGrid2Mvc.Samples.Models.jsGridSamples;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace jsGrid2Mvc.Samples.Controllers
{
	public class jsGridSamplesController : Controller
	{
		readonly IConfiguration _Config;
		readonly GridSettings _Settings;

		public jsGridSamplesController(IConfiguration config)
		{
			//this._Settings = settings ?? throw new ArgumentNullException(nameof(settings));

			this._Config = config ?? throw new ArgumentNullException(nameof(config));
		}

		public IActionResult StaticData()
		{
			return View(new StaticDataModel());
		}

		public IActionResult ControllerData()
		{
			return View(new ControllerModel(this._Config));
		}
	}
}
