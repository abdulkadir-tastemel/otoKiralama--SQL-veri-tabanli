using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using RentCars.Models;
using System.Data.Entity;

namespace RentCars.Controllers
{
    public class CustomerController : Controller
    {

        CarsDbEntities db = new CarsDbEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }



        public static string GetMD5_2(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] formData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(formData);
            string byte25String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte25String += targetData[i].ToString("x2");
            }
            return byte25String;


        }

        public ActionResult Giris()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Giris(FormCollection bilgi)
        {

            string tc = bilgi["tc"].ToString();
            string sif = GetMD5_2(bilgi["sif"].ToString());
            var count = db.Musteriler.Where(x => x.Tckimlik == tc && x.sifre == sif).Count();




            if (count == 0)
            {
                ViewData["sonuc"] = "HATA kayıtlı değilsin veya bilgilerin yanlış";
            }

            else
            {
                Session["session_giris"] = true;
                Session["session_tc"] = tc;
              //  ViewData["sonuc"] = "tebrikler...";
                return RedirectToAction("Profil", "Customer");
            }
            return View();
        }



        public ActionResult Kayit()
        {
            return View();
        }



        [HttpPost]

        public ActionResult Kayit([Bind(Include ="Tckimlik,adsoyad,dogumtarihi,cinsiyet,telefon,sifre")] Musteriler musteriler)
        {

            db.Musteriler.Add(musteriler);

            int result = db.SaveChanges();
            if (result>0)
            {
                ViewData["sonuc"] = "Tebrikler kaydınız gerçekleşti.......";
            }
            else
            {
                ViewData["sonuc"] = "HATA !! kaydınız gerçekleşmedi.......";
            }

            return View();
        }


        [HttpGet]

        public ActionResult Profil()
        {

            if (Session["session_giris"] != null)
            {

                string tc = Session["session_tc"].ToString();
                Musteriler musteriler = db.Musteriler.Find(tc);

                if (musteriler == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(musteriler);
                }


            }
            else
            {
                return RedirectToAction("Giris");
            }
        }


        [HttpPost]
        public ActionResult Profil(FormCollection bilgi)
        {


            if (ModelState.IsValid)
            {
                string tc = Session["session_tc"].ToString();
                Musteriler musteri = db.Musteriler.Find(tc);

                musteri.adsoyad = bilgi["adsoyad"].ToString();
                musteri.dogumtarihi = Convert.ToDateTime(bilgi["dogumtarihi"]);
                musteri.cinsiyet = bilgi["cinsiyet"].ToString();
                musteri.telefon = bilgi["telefon"].ToString();

                db.Entry(musteri).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Profil");
            }



            return View();
        }


        public ActionResult Cikis()
        {
            Session.Remove("session_tc");
            Session.Clear();

            return RedirectToAction("Giris");
        }


        public ActionResult SifreDegistir()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifreDegistir(FormCollection bilgi)
        {


            string tc = Session["session_tc"].ToString();
            string sif = GetMD5_2(bilgi["msif"].ToString());
            var count = db.Musteriler.Where(x => x.Tckimlik == tc && x.sifre == sif).Count();


            if (count>0)
            {

                if (bilgi["ysif"] == bilgi["ysif2"])
                {
                    string ysif = GetMD5_2(bilgi["ysif"].ToString());
                    Musteriler musteri = db.Musteriler.Find(tc);
                    musteri.sifre = ysif;
                    db.Entry(musteri).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewData["sonuc"] = "TEBRİKLER ŞİFRE DEĞİŞTİ ....";

                }
                else
                {
                    ViewData["sonuc"] = "HATA !! YENİ ŞİFRELER UYŞMUYOR....";
                }





            }
            else
            {
                ViewData["sonuc"] = "HATA MEVCUT ŞİFRE YANLIŞ...";
            }


            return View();
        }


    }
}