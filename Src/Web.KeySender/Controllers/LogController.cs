using System.Web.Mvc;
using Web.KeySender.Models;

namespace Web.KeySender.Controllers
{
    public class LogController : Controller
    {
        public ActionResult Index() => View(new LogModel(Config.IsLoggingEnabled));
    }
}