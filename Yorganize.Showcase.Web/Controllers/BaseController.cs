using System.Web.Mvc;

namespace Yorganize.Showcase.Web.Controllers
{
    public class BaseController : Controller
    {
      
        protected void Alert(string message, string type)
        {
            TempData.Add("alert-type", type);
            TempData.Add("message", message);
        }

    }
}
