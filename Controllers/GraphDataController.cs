using DashBoard.Web.Domain;
using DashBoard.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Controllers
{
    public class GraphDataController : Controller
    {
        // GET: GraphData
        public ActionResult Index()
        {
            using (var context = new AWorksEntities())
            {
                
                       var sales = context.f_uspGetTotalYearlySales();
                       
                        


                       var totalSales = from e in sales
                           select new YearlySales
                           {
                               Year = e.Year.GetValueOrDefault(),
                               Sales = (decimal) e.TotalSales.GetValueOrDefault()
                               
                           };
                List<YearlySales> ys = new List<YearlySales>(totalSales.ToList());

                  
                var test = Json(ys, JsonRequestBehavior.AllowGet);
            }
            
            return View();
        }
        [HttpGet]
        public JsonResult GetSalesGraphData()
        {             using (var context = new AWorksEntities())
            {
                
                       var sales = context.f_uspGetTotalYearlySales();
                        List<YearlySales> ys = sales.Select(s=> new YearlySales
                        {
                            Year = s.Year.GetValueOrDefault(),
                            Sales = s.TotalSales.GetValueOrDefault()
                    
                        }).ToList();
                return Json(ys, JsonRequestBehavior.AllowGet);
        
            }
                    
        
        }

        public ViewResult testChart()
        {
            return View();
        }
        

    }
}