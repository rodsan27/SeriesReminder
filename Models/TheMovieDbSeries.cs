using SERIESREMINDER.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SERIESREMINDER.Models
{
    public class TheMovieDbSeries
    {
        [Required]
        public string searchText { get; set; }
        public string original_name { get; set; }
        public List<object> genre_ids { get; set; }
        public string name { get; set; }
        public double popularity { get; set; }
        public List<string> origin_country { get; set; }
        public double vote_count { get; set; }
        public string first_air_date { get; set; }
        public string backdrop_path { get; set; }
        public string original_language { get; set; }
        public int id { get; set; }
        public double vote_average { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string last_air_date { get; set; }
        public object next_episode_to_air { get; set; }
        public int number_of_seasons { get; set; }
        public string status { get; set; }
    }
}