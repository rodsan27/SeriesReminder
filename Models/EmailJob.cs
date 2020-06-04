using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Net;
using System.Net.Mail;
using Quartz.Impl;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using SERIESREMINDER.Class;
using System.Web.Mvc;

namespace SERIESREMINDER.Models
{
    public class EmailJob : IJob
    {
        string apiKey = "794094135654cd2d9baf6fd5b960372b";
        private SeriesReminderEntities db = new SeriesReminderEntities();
        string email = "", pass = "", host = "", listaCorreos = "", body = "";
        int port = 0;
        static int hour = 0, minute = 0;
        public void getCredenciales()
        {
            listaCorreos = db.listaCorreo().FirstOrDefault();
            var bodyParam = new ObjectParameter("Body", typeof(string));
            db.cuerpoCorreo(bodyParam);
            body = (string)bodyParam.Value;
            var credencialesCorreo = db.credencialesCorreo().FirstOrDefault();
            email = credencialesCorreo.Correo;
            pass = credencialesCorreo.Contrasena;
            host = credencialesCorreo.Host;
            port = Convert.ToInt32(credencialesCorreo.Port) == 0 ? 25 : Convert.ToInt32(credencialesCorreo.Port);
            hour = Convert.ToInt32(credencialesCorreo.Hora);
            minute = Convert.ToInt32(credencialesCorreo.Minutos);
        }

        private void updateSeries()
        {
            string valorParametro = "U";
            var parametro = new SqlParameter("@parametro", valorParametro);
            var listaIDSerie = db.Database.SqlQuery<CatSerie>("seriesDia @parametro", parametro).ToList();
            foreach (var item in listaIDSerie)
            {
                int id = item.IDSerie;
                string estado = "";
                string ultimoEpisodio, proximoEpisodio;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
               | SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12;

                HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/tv/" + id + "?api_key=" + apiKey + "&language=es-MX") as HttpWebRequest;
                string apiResponse = "";              

                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }

                ResponseSerie rootObject = JsonConvert.DeserializeObject<ResponseSerie>(apiResponse);
                estado = rootObject.status;
                ultimoEpisodio = rootObject.last_air_date;
                proximoEpisodio = rootObject.next_episode_to_air == null ? "0001-01-01" : rootObject.next_episode_to_air.ToString().Substring(18, 10);

                try
                {
                    db.actualizarFechaSeries(id, estado, Convert.ToDateTime(ultimoEpisodio), Convert.ToDateTime(proximoEpisodio));
                }
                catch (Exception ex)
                {
                    View("Error", new HandleErrorInfo(ex, "Home", "Index"));
                }

            }
        }

        private void View(string v, HandleErrorInfo handleErrorInfo)
        {
            throw new NotImplementedException();
        }

        public void Execute(IJobExecutionContext context)
        {
            getCredenciales();

            if (listaCorreos == null || email == null || pass == null || host == null || port == 0)
            {
                return;
            }

            var message = new MailMessage();
            foreach (string correo in listaCorreos.Split(','))
            {
                message.To.Add(new MailAddress(correo));
            }

            message.From = new MailAddress(email);
            message.Subject = "Lista de Series";
            message.Body = body;
            message.IsBodyHtml = true;
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = host,
                Port = port,
                Credentials = new NetworkCredential(email, pass)
            })
            {
                client.Send(message);
            }
        }
        public void JobStart()
        {
            updateSeries();
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                  //s.WithIntervalInMinutes(1)
                    s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}