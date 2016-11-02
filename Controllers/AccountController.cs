//#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.ObjectModel;
using DashBoard.Web.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Security.Principal;
using DashBoard.Web.Models;
#if DEBUG
using Moq;
#else
#endif

namespace DashBoard.Web.Controllers
{
    public class AccountController : Controller
    {
#if DEBUG
        enum Test { AccountingAdmin = 1, AccountingUser, PDAdmin, PDUser, UMSAdmin, UMSUser, MSAdmin, MSUser, sysadmin }
#else
#endif

        private IPrincipal _user;
        public AccountController(IPrincipal oUser)
        {
            _user = oUser;
        }
        public AccountController()
        {

        }


        // GET: Account
        public ActionResult Index()
        {


            return View();
        }

        public string ADBulkImport()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ParksEmployeeEntities db = new ParksEmployeeEntities();
            db.uspDeleteADUser();
            int cntUsers = 0;

            using (db)
            {


                ADManager am = new ADManager();
                var test = am.Groups();
                foreach (var itemGroup in test)
                {

                    var name = itemGroup.ToString();

                    var users = am.Users(name);

                    foreach (var itemUser in users)
                    {
                        if (itemUser.StructuralObjectClass != null && itemUser.StructuralObjectClass.ToUpper() == "USER")
                        {


                            var userData = new ADUser();

                            userData.GUID = itemUser.Guid.ToString();

                            if (((System.DirectoryServices.AccountManagement.UserPrincipal)(itemUser)).EmailAddress == null)
                            {
                                userData.Email = "";
                            }
                            else
                            {
                                userData.Email = ((System.DirectoryServices.AccountManagement.UserPrincipal)(itemUser)).EmailAddress.ToString();
                            }



                            if (((System.DirectoryServices.AccountManagement.UserPrincipal)(itemUser)).VoiceTelephoneNumber == null)
                            {
                                userData.PhoneNumber = "";
                            }
                            else
                            {
                                userData.PhoneNumber = ((System.DirectoryServices.AccountManagement.UserPrincipal)(itemUser)).VoiceTelephoneNumber.ToString();
                            }

                            userData.UserName = itemUser.SamAccountName.ToString();
                            if (itemUser.DisplayName != null)
                            {
                                userData.DisplayName = itemUser.DisplayName.ToString();
                            }
                            else
                            {
                                userData.DisplayName = "";
                            }

                            userData.GroupName = name;

                            try
                            {
                                db.ADUsers.Add(userData);


                                db.SaveChanges();
                                cntUsers++;
                            }

                            catch (DbEntityValidationException dbEx)
                            {
                                foreach (var validationErrors in dbEx.EntityValidationErrors)
                                {
                                    foreach (var validationError in validationErrors.ValidationErrors)
                                    {
                                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                                validationError.PropertyName,
                                                                validationError.ErrorMessage);
                                    }
                                }
                            }


                            finally
                            {

                            }




                        }



                    }

                }
            }
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);


            return cntUsers.ToString() + " Users are imported DB during " + elapsedTime;
        }
        public class ADManager
        {
            private ContextType _Context;

            public ADManager(ContextType context)
            {
                this._Context = context;
                this.StartUp();
            }

            public ADManager()
            {
                this._Context = ContextType.Domain;
                this.StartUp();
            }

            public static string CurrentDomain()
            {
                string name;
                try
                {
                    name = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().Name;

                }
                catch (ActiveDirectoryObjectNotFoundException activeDirectoryObjectNotFoundException)
                {
                    name = Environment.MachineName;
                }
                catch (ActiveDirectoryOperationException activeDirectoryOperationException)
                {
                    name = Environment.MachineName;
                }
                return name;
            }

            public ReadOnlyCollection<string> Groups()
            {
                PrincipalSearcher principalSearcher = new PrincipalSearcher()
                {
                    QueryFilter = new GroupPrincipal(new PrincipalContext(this._Context))
                };
                PrincipalSearchResult<Principal> principals = principalSearcher.FindAll();
                List<string> strs = new List<string>();
                foreach (Principal principal in principals)
                {
                    strs.Add(principal.SamAccountName);
                }
                strs.Sort();
                return new ReadOnlyCollection<string>(strs);
            }

            private static bool IsMemberOfDomain()
            {
                bool flag = false;
                try
                {
                    System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
                    flag = true;
                }
                catch (ActiveDirectoryObjectNotFoundException activeDirectoryObjectNotFoundException)
                {
                }
                return flag;
            }

            private void StartUp()
            {
                if (!ADManager.IsMemberOfDomain())
                {
                    this._Context = ContextType.Machine;
                }
            }

            internal List<Principal> Users(string groupName)
            {
                PrincipalSearcher principalSearcher = new PrincipalSearcher();
                GroupPrincipal groupPrincipal = new GroupPrincipal(new PrincipalContext(this._Context))
                {
                    SamAccountName = groupName
                };
                principalSearcher.QueryFilter = groupPrincipal;
                GroupPrincipal groupPrincipal1 = (GroupPrincipal)principalSearcher.FindOne();
                List<Principal> principals = new List<Principal>();
                principals.AddRange(groupPrincipal1.Members);
                return principals;
            }
        }

        public List<string> getGroupName()
        {
            var result = new List<string>();
            var userName = System.Web.HttpContext.Current.User;

            // Execute ADO Process


            // Get Result 

            return result;


        }

        public string[] GetGroups()
        {
            string tempusername;
            string username;
            string[] output = null;

            UserInjectionMethod(out tempusername, out username);


            using (var ctx = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(ctx, username))
            {
                if (user != null)
                {
                    output = user.GetGroups() //this returns a collection of principal objects
                        .Select(x => x.SamAccountName) // select the name.  you may change this to choose the display name or whatever you want
                        .ToArray(); // convert to string array
                }
            }

            return output;
        }

        private void UserInjectionMethod(out string tempusername, out string username)
        {
            try
            {
#if DEBUG
                tempusername = _user.Identity.Name;
#else
        tempusername = (System.Web.HttpContext.Current.User.Identity.Name);
#endif


            }
            catch (Exception ex)
            {

                tempusername = _user.Identity.Name;
            }


            username = tempusername;
        }



        public ActionResult GetGroupNameView()
        {

            var result = GetGroupsJson();
            string jsResult = new JavaScriptSerializer().Serialize(result.Data);
            groupViewModel vm = JsonConvert.DeserializeObject<groupViewModel>(jsResult);
            //var vm = new groupViewModel();
            //var tst = new JavaScriptSerializer().Deserialize<groupViewModel>(result);


            //vm = GetGroupsJson();
            //return View(vm);  
            //return View(vm);
            return View(vm);
        }
        public JsonResult GetGroupsJson()
        {
            string[] output = null;
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            using (var ctx = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(ctx, username))
            {
                if (user != null)
                {
                    output = user.GetGroups() //this returns a collection of principal objects
                        .Select(x => x.Name) // select the name.  you may change this to choose the display name or whatever you want
                        .ToArray(); // convert to string array
                }
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetGroupsJsonClass()
        {

            object output = null;
            string tempusername;
            string username;

            UserInjectionMethod(out tempusername, out username);
            //string username = System.Web.HttpContext.Current.User.Identity.Name;
            using (var ctx = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(ctx, username))
            {
                if (user != null)
                {
                    //output = user.GetGroups() //this returns a collection of principal objects
                    output = user.GetGroups()
                           .Select(x => new groupViewModelClass { Name = x.Name, Description = x.SamAccountName }) // select the name.  you may change this to choose the display name or whatever you want

                           .ToArray(); // convert to string array
                }
            }

            return View(output);

        }

        public JsonResult GetGroupsJsonUsingModel()
        {

            IEnumerable<groupViewModelClass> output = null;
            string tempusername;
            string username;

            username = TestUser();

            using (var ctx = new PrincipalContext(ContextType.Domain))
            using (var user = UserPrincipal.FindByIdentity(ctx, username))
            {
                if (user != null)
                {
                    //output = user.GetGroups() //this returns a collection of principal objects
                    output = user.GetGroups()
                           .Select(x => new groupViewModelClass { Name = x.Name, Description = x.SamAccountName }) // select the name.  you may change this to choose the display name or whatever you want
                        //.Select(x => x.SamAccountName)
                           .ToArray(); // convert to string array
                }
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        private string TestUser()
        {
            string username;
            
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
            username = _user.Identity.Name;

#else

            string tempusername;
            UserInjectionMethod(out tempusername, out username);
            
#endif
            
            return username;
        }

        public JsonResult GetGroupJsonUsingModel(IPrincipal _user, PrincipalContext _ctx)
        {
            IEnumerable<groupViewModelClass> output = null;
            string username = _user.Identity.Name;
            using (_ctx)
            {
                using (var user = UserPrincipal.FindByIdentity(_ctx, username))
                {
                    output = user.GetGroups()
                        .Select(x => new groupViewModelClass { Name = x.Name, Description = x.SamAccountName }) // select the name.  you may change this to choose the display name or whatever you want
                        //.Select(x => x.SamAccountName)
                        .ToArray(); // convert to string array

                }
            }

            return Json(output, JsonRequestBehavior.AllowGet);
        }



    }

}