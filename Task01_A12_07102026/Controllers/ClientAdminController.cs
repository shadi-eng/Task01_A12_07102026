using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task01_A12_07102026.Models;

namespace Task01_A12_07102026.Controllers
{
   
    public class ClientAdminController : Controller
    {
        [Authorize(Roles = "ClientAdmin")]
        public IActionResult Index()
        {
            //I know that the user is authenticated and has the "ClientAdmin" role, so I can return the view for the ClientAdmin dashboard.
            //who is the user? I can get the user ID from the claims.
            int ClientID = int.Parse(User.Identity.Name);
            Client c1=Client.GetClientById(ClientID);
            ViewBag.ClientName = c1.ClientName;
            return View();
        }

        [Authorize(Roles = "ClientAdmin")]
        public IActionResult UpdateView()
        {
            return View();
        }
    }
}
