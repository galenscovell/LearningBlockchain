using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PayChain.Wallet.Controllers
{
    public class CoreController : Controller
    {
        public CoreController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
