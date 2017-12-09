using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PayChain.Wallet.Controllers
{
    public class CoreController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
