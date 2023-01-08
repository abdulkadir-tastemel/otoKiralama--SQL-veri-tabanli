using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentCars.Models;

namespace RentCars.Controllers
{
    public class rezervationController : Controller
    {

        CarsDbEntities db = new CarsDbEntities();
        // GET: rezervation
        public ActionResult Index()
        {
            if (Session["session_giris"] != null)
            {
                string tc = Session["session_tc"].ToString();
                return View(db.rezervasyonlar.Where(x => x.tc == tc).ToList());

              
            }

            else
            {
                return RedirectToAction("Giris", "Customer");
            }
        }
    }
}