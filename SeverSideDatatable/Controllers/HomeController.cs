using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeverSideDatatable.Controllers
{
    public class HomeController : Controller
    {
        jayaEntities db = new jayaEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(JQueryDatatableParam param)
        {
            IEnumerable<Customer> customers = db.Customers;
            //customers.ToList().ForEach(x => x.BirthdateString = x.Birthdate.ToString());
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                customers = customers.Where(x => x.Name!=null && x.Name.ToLower().Contains(param.sSearch.ToLower())
                                              || x.Address!= null && x.Address.ToLower().Contains(param.sSearch.ToLower())
                                              || x.Mobileno!= null && x.Mobileno.ToLower().Contains(param.sSearch.ToLower())
                                              || x.EmailID!=null && x.EmailID.ToString().Contains(param.sSearch.ToLower())
                                              || x.Birthdate !=null && x.Birthdate.ToString().Contains(param.sSearch.ToLower()));
            }
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
            if (sortColumnIndex == 3)
            {
                customers = sortDirection == "asc" ? customers.OrderBy(c => c.Birthdate) : customers.OrderByDescending(c => c.Birthdate);
            }
            else
            {
                Func<Customer, string> orderingFunction = e => sortColumnIndex == 0 ? e.Name :
                                                               sortColumnIndex == 1 ? e.Address :
                                                               e.EmailID;
                customers = sortDirection == "asc" ? customers.OrderBy(orderingFunction) : customers.OrderByDescending(orderingFunction);
            }

            var displayResult = customers.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = customers.Count();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
    
}