//#define DEBUG
using DashBoard.Web.Domain;
using DashBoard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoard.Web.Models;
using System.Security.Principal;
using System.Configuration;
#if DEBUG
using Moq;
using System.DirectoryServices.AccountManagement;
#else
#endif


namespace DashBoard.Web.Controllers
{

    public class NavbarController : Controller
    {

#if DEBUG
        enum Test { AccountingAdmin = 1, AccountingUser, PDAdmin, PDUser, UMSAdmin, UMSUser, MSAdmin, MSUser, sysadmin }
#else
#endif

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


            // please extend this section to filter menu sidebar

            // Below is to Add Admin Function
            List<Navbar> list = null;

            TestUser();

            var result = new AccountController(_user).GetGroups();

            if (result.Any(a => a.Contains("Systems Develo")
                || result.Any(b => b.Contains("Admin Manager")
                )))
            {
                list = data.navbarItems().ToList();
            }
            else
            {
                list = data.navbarItems().ToList();
                var item = list.SingleOrDefault(x => x.Id == 18);
                if (item != null)
                {
                    list.Remove(item);
                }

            }
            return PartialView("_Navbar", list);
            //The above code is going to View

            //return PartialView("_Navbar", data.navbarItems().ToList());

        }

        private void TestUser()
        {
#if DEBUG
            int caseSwitch = Properties.Settings.Default.TestCase;

            GenericIdentity fakeIdentiy = null;
            switch (caseSwitch)
            {

                case (int)Test.AccountingAdmin:
                    // Below is AcctAdmin
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.acctAdmin);
                    break;
                case (int)Test.AccountingUser:
                    // Below is AcctUser
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.acctUser);
                    break;

                case (int)Test.PDAdmin:
                    // Below is PDAdmin
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.pndAdmin);
                    break;

                case (int)Test.PDUser:
                    // Below is PDtUser
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.pndUser);
                    break;

                case (int)Test.UMSAdmin:

                    // Below is UMSAdmin
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.umsAdmin);
                    break;


                case (int)Test.UMSUser:
                    // Below is UMStUser
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.umsUser);
                    break;

                case (int)Test.MSAdmin:
                    // Below is MStAdmin
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.msAdmin);
                    break;

                case (int)Test.MSUser:
                    // Below is MSUser
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.msUser);
                    break;
                case (int)Test.sysadmin:
                    // Below is MSUser
                    fakeIdentiy = new GenericIdentity(Properties.Settings.Default.sysadmin);
                    break;

            }


            GenericPrincipal principal = new GenericPrincipal(fakeIdentiy, null);
            _user = principal;

#else
#endif
        }

    }
}