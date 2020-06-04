using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using SERIESREMINDER.Models;

namespace SERIESREMINDER.Controllers
{
    public class CatCorreosController : Controller
    {
        private SeriesReminderEntities db = new SeriesReminderEntities();

        // GET: CatCorreos
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Index()
        {
            return View(db.CatCorreos.ToList());
        }

        // GET: CatCorreos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatCorreo catCorreo = db.CatCorreos.Find(id);
            if (catCorreo == null)
            {
                return HttpNotFound();
            }
            return View(catCorreo);
        }

        // GET: CatCorreos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CatCorreos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCorreo,Correo,Estado")] CatCorreo catCorreo)
        {
            if (ModelState.IsValid)
            {
                db.CatCorreos.Add(catCorreo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catCorreo);
        }

        // GET: CatCorreos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatCorreo catCorreo = db.CatCorreos.Find(id);
            if (catCorreo == null)
            {
                return HttpNotFound();
            }
            return View(catCorreo);
        }

        // POST: CatCorreos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCorreo,Correo,Estado")] CatCorreo catCorreo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catCorreo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catCorreo);
        }

        // GET: CatCorreos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatCorreo catCorreo = db.CatCorreos.Find(id);
            if (catCorreo == null)
            {
                return HttpNotFound();
            }
            return View(catCorreo);
        }

        // POST: CatCorreos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CatCorreo catCorreo = db.CatCorreos.Find(id);
            db.CatCorreos.Remove(catCorreo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
