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
    public class ConfigCorreosController : Controller
    {
        private SeriesReminderEntities db = new SeriesReminderEntities();

        // GET: ConfigCorreos
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Index()
        {
            return View(db.ConfigCorreos.ToList());
        }

        // GET: ConfigCorreos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfigCorreo configCorreo = db.ConfigCorreos.Find(id);
            if (configCorreo == null)
            {
                return HttpNotFound();
            }
            return View(configCorreo);
        }

        // GET: ConfigCorreos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConfigCorreos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDConfig,correo,contrasena,host,puerto,hora,minutos")] ConfigCorreo configCorreo)
        {
            if (ModelState.IsValid)
            {
                db.ConfigCorreos.Add(configCorreo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(configCorreo);
        }

        // GET: ConfigCorreos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfigCorreo configCorreo = db.ConfigCorreos.Find(id);
            if (configCorreo == null)
            {
                return HttpNotFound();
            }
            return View(configCorreo);
        }

        // POST: ConfigCorreos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDConfig,correo,contrasena,host,puerto,hora,minutos")] ConfigCorreo configCorreo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configCorreo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(configCorreo);
        }

        // GET: ConfigCorreos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfigCorreo configCorreo = db.ConfigCorreos.Find(id);
            if (configCorreo == null)
            {
                return HttpNotFound();
            }
            return View(configCorreo);
        }

        // POST: ConfigCorreos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConfigCorreo configCorreo = db.ConfigCorreos.Find(id);
            db.ConfigCorreos.Remove(configCorreo);
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
