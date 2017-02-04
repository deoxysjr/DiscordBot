﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.NadekoBot
{
    public class AnimeResult
    {
        public int id;
        public string AiringStatus => airing_status;
        public string airing_status;
        public string title_english;
        public int total_episodes;
        public string description;
        public string image_url_lge;
        public string[] Genres;
        public string average_score;

        public string Link => "http://anilist.co/anime/" + id;
        public string Synopsis => description?.Substring(0, description.Length > 500 ? 500 : description.Length) + "...";
    }
}
