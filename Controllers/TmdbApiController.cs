using Newtonsoft.Json;
using SERIESREMINDER.Class;
using SERIESREMINDER.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SERIESREMINDER.Controllers
{
    public class TmdbApiController : Controller
    {
        // GET: TmdbApi
        
        string apiKey = "794094135654cd2d9baf6fd5b960372b";
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Index(string peopleName, int? page)
        {
            if (page != null)
                CallAPISeries(peopleName, Convert.ToInt32(page));

            Models.TheMovieDbSeries theMovieDb = new Models.TheMovieDbSeries();
            theMovieDb.searchText = peopleName;
            return View(theMovieDb);
        }

        [HttpPost]
        public ActionResult Index(Models.TheMovieDbSeries theMovieDb, string searchText)
        {
            if (ModelState.IsValid)
            {
                CallAPISeries(searchText, 0);
            }
            return View(theMovieDb);
        }

        public void CallAPISeries(string searchText, int page)
        {
            int pageNo = Convert.ToInt32(page) == 0 ? 1 : Convert.ToInt32(page);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12;

            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/search/tv?api_key=" + apiKey + "&language=es-MX&query=" + searchText + "&page=" + pageNo) as HttpWebRequest;
            string apiResponse = "";   
        
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
                response.Close();
            }

            ResponseSearchSeries rootObject = JsonConvert.DeserializeObject<ResponseSearchSeries>(apiResponse);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"resultDiv\"><p>Series</p>");

            foreach (Results result in rootObject.results)
            {
                string image = result.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + result.poster_path;
                string link = Url.Action("GetSerie", "TmdbApi", new { id = result.id });

                sb.Append("<div class=\"result\" resourceId=\"" + result.id + "\">" + "<a href=\"" + link + "\"><img src=\"" + image + "\" / >" + "<p>" + result.name + "</a></p></div>");
            }

            ViewBag.Result = sb.ToString();

            int pageSize = 20;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = pageNo;
            pagingInfo.TotalItems = rootObject.total_results;
            pagingInfo.ItemsPerPage = pageSize;
            ViewBag.Paging = pagingInfo;
        }

        public ActionResult GetSerie(int id)
        {
            {
                HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/tv/" + id + "?api_key=" + apiKey + "&language=es-MX") as HttpWebRequest;
                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }

                ResponseSerie rootObject = JsonConvert.DeserializeObject<ResponseSerie>(apiResponse);
                TheMovieDbSeries theMovieDbSeries = new TheMovieDbSeries();
                theMovieDbSeries.name = rootObject.name;
                theMovieDbSeries.overview = rootObject.overview;
                theMovieDbSeries.poster_path = rootObject.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + rootObject.poster_path;
                theMovieDbSeries.id = rootObject.id;
                theMovieDbSeries.number_of_seasons = rootObject.number_of_seasons;
                theMovieDbSeries.last_air_date = rootObject.last_air_date;
                theMovieDbSeries.next_episode_to_air = rootObject.next_episode_to_air == null ? "" : rootObject.next_episode_to_air.ToString().Substring(18, 10);
                theMovieDbSeries.status = rootObject.status;

                return View(theMovieDbSeries);
            }
        }

        /********************* GUARDADO EN LA BASE DE DATOS *********************************/
        private SeriesReminderEntities db = new SeriesReminderEntities();
        public ActionResult Create([Bind(Include = "IDSerie,nombreSerie,resumenSerie,posterPath,Estado,cantTemporadas,ultimoEpisodio,proximoEpisodio")] CatSerie catSerie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.CatSeries1.Add(catSerie);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "TmdbApi", "Create"));
                }
              
            }

            return View(catSerie);
        }      
    }
}