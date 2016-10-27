using DashBoard.Web.Domain;
using DashBoard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoard.Web.Models;
using System.Security.Principal;

namespace DashBoard.Web.Controllers
{
    public class NavbarController : Controller
    {
        private IPrincipal _user;
        public NavbarController(IPrincipal oUser)
        {
            _user = oUser;
        }
        public NavbarController()
        { 
        }
        // GET: Navbar
        public ActionResult Index()
        {
            
            var data = new Data();
            List<Navbar> list = null;

            // please extend this section to filter menu sidebar

            var result = new AccountController(_user).GetGroups();

            if (result.Any(a => a.Contains ("Systems Develo")
                || result.Any(b => b.Contains ("Admin Manager")
                )))
            {
                list = data.navbarItems().ToList();
            }
            else
            {
                list = data.navbarItems().ToList();
                var item = list.SingleOrDefault(x => x.Id == 18);
                 if (item !=null)
                 {
                     list.Remove(item);
                 }

            }
            //return PartialView("_Navbar", data.navbarItems().ToList());
            return PartialView("_Navbar", list);
        }
   
    }
}