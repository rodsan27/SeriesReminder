using SERIESREMINDER.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace SERIESREMINDER.Controllers
{
    public class HomeController : Controller
    {
        private SeriesReminderEntities db = new SeriesReminderEntities();
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(PerfilUsuario objUser)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    var obj = db.PerfilUsuarios.Where(a => a.Usuario.Equals(objUser.Usuario) && a.Contrasena.Equals(objUser.Contrasena)).FirstOrDefault();
                    if (obj != null)
                    {
                        FormsAuthentication.SetAuthCookie(objUser.Usuario, false);
                        Session["UserID"] = obj.IDUser.ToString();
                        Session["UserName"] = obj.Usuario.ToString();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session.Abandon();           
            return RedirectToAction("Login", "Home");
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                try
                {
                    string valorParametro = "S";
                    var parametro = new SqlParameter("@parametro", valorParametro);
                    var serie = db.Database.SqlQuery<CatSerie>("seriesDia @parametro", parametro).ToList();
                    return View(serie);
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Home", "Index"));
                }
            }
            else
            {
                return RedirectToAction("Login");
            }          
           
        }
    }
}
