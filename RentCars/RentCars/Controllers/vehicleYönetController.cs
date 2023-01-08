using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCars.Models;
using System.IO;

namespace RentCars.Controllers
{
    public class vehicleYönetController : Controller
    {
        private CarsDbEntities db = new CarsDbEntities();

        // GET: vehicleYönet
        public ActionResult Index()
        {
            return View(db.carsTable.ToList());
        }

        // GET: vehicleYönet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carsTable carsTable = db.carsTable.Find(id);
            if (carsTable == null)
            {
                return HttpNotFound();
            }
            return View(carsTable);
        }

        // GET: vehicleYönet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: vehicleYönet/Create
        // Aşırı gönderim saldırılarından korunmak için, bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CarsName,CarsModel,carsDate,carsGear,carsGas,carsPrice")] carsTable carsTable)
        {
            if (ModelState.IsValid)
            {
                db.carsTable.Add(carsTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carsTable);
        }

        // GET: vehicleYönet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carsTable carsTable = db.carsTable.Find(id);
            if (carsTable == null)
            {
                return HttpNotFound();
            }
            return View(carsTable);
        }

        // POST: vehicleYönet/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CarsName,CarsModel,carsDate,carsGear,carsGas,carsPrice")] carsTable carsTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carsTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carsTable);
        }

        // GET: vehicleYönet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carsTable carsTable = db.carsTable.Find(id);
            if (carsTable == null)
            {
                return HttpNotFound();
            }
            return View(carsTable);
        }

        // POST: vehicleYönet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            carsTable carsTable = db.carsTable.Find(id);
            db.carsTable.Remove(carsTable);
            db.SaveChanges();

            string ImageFileName = id.ToString() + ".jpg";
            string FolderPath = Path.Combine(Server.MapPath("~/VehicleImages"), ImageFileName);

            if (System.IO.File.Exists(FolderPath))
            {
                System.IO.File.Delete(FolderPath);

            }

            return RedirectToAction("Index");
        }


        [HttpGet]

        public ActionResult SavageImages()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavageImages(string hiddenId, HttpPostedFileBase UploadedImage)
        {
            if (UploadedImage.ContentLength > 0)
            {
                string ImageFileName = hiddenId + ".jpg";
                string FolderPath = Path.Combine(Server.MapPath("~/VehicleImages"), ImageFileName);
                UploadedImage.SaveAs(FolderPath);
            }
            ViewBag.Message = hiddenId + ".jpg İSİMLİ RESİM BAŞARIYLA YÜKLENDİ";

            return View();
        }
      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
