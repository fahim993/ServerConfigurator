using Microsoft.AspNetCore.Mvc;
using ServerConfigurator.Model;
using ServerConfigurator.Service;
using ServerConfigurator.Web.Models;
using System.Diagnostics;

namespace ServerConfigurator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfigService _configService;

        public HomeController(ILogger<HomeController> logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
        }

        public IActionResult Index()
        {
            var configModel = _configService.GetConfigModel();

            return View(configModel);
        }

        [HttpPost]
        public IActionResult UpdateConfig(List<string> defaultKeys, List<string> defaultValues, List<string> serverNames, List<string> serverKeys, List<string> serverValues)
        {
            // Create a new ConfigModel object
            var configModel = new ConfigModel();

            // Populate the DefaultValues dictionary
            for (int i = 0; i < defaultKeys.Count; i++)
            {
                configModel.DefaultValues[defaultKeys[i]] = defaultValues[i];
            }

            // Assume that serverNames, serverKeys, and serverValues are of the same length
            // and each index corresponds to a unique server configuration
            for (int i = 0; i < serverNames.Count; i++)
            {
                if (!configModel.ServerConfigs.ContainsKey(serverNames[i]))
                {
                    configModel.ServerConfigs[serverNames[i]] = new Dictionary<string, string>();
                }

                configModel.ServerConfigs[serverNames[i]][serverKeys[i]] = serverValues[i];
            }

            // Uncomment the following line if you want to save the updated configuration
            _configService.SetConfigModel(configModel);

            // Redirect to the Index action method to display the updated configuration
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
