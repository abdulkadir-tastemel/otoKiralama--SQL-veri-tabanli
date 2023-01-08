using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentCars.Models;

namespace RentCars.Controllers
{
    public class vehicleController : Controller
    {

        CarsDbEntities db = new CarsDbEntities();
        // GET: vehicle
        public ActionResult Index()
        {
            return View(db.carsTable.ToList());
        }

        public ActionResult Details(int ? id)
        {
            carsTable aracbilgisi = db.carsTable.Find(id);
            return View(aracbilgisi);
        }


        [HttpGet]
        public ActionResult rezervation(int? id)
        {
            carsTable aracbilgisi = db.carsTable.Find(id);
            ViewData["CarsName"] = aracbilgisi.CarsName;
            ViewData["CarsModel"] = aracbilgisi.CarsModel;
            ViewData["carsPrice"] = aracbilgisi.carsPrice;
            return View();
        }

        [HttpPost]

        public ActionResult rezervation([Bind(Include ="rezervasyonId,aracıd,tc,adsoyad,AlmaTarihi,TeslimTarihi,ucret")] rezervasyonlar rezervasyonlarr)
        {

            if (ModelState.IsValid)
            {
                db.rezervasyonlar.Add(rezervasyonlarr);
                db.SaveChanges();
            }

            ViewBag.Message = "TEBRİKLER REZERVASYON İŞLEMİ BAŞARILI OLDU";
            return View();


        }                   


    }
}