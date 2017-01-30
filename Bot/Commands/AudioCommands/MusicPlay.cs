﻿using Discord.Commands;
using System;
using System.Net;
using System.Text.RegularExpressions;
using YoutubeSearch;
using Discord;

namespace Bot.Commands.AudioCommands
{
    class MusicPlay : BotCommand
    {

        MyBot myBot;
        WebClient webClient;

        public MusicPlay(MyBot myBot) : base("music play", "Play music", "music play <url>", new string[] { "play" })
        {
            this.myBot = myBot;
            this.webClient = new WebClient();
            //onCommand();
        }

        public void onCommand()
        {
            var items = new VideoSearch();

            myBot.discord.GetService<CommandService>().CreateCommand("music play").Alias(new string[] { "play", "m play" })
                .Parameter("url", ParameterType.Unparsed)
                .Do(async (e) =>
            {
                e.Channel.SendMessage("Searching for first video...");
                string query = e.GetArg("url").Replace("_", " ");
                myBot.audioManager.play(e, e.User, getVideoUrl(query));
                int pages = 1;

                //Code gods forgive me pls
                /*
                foreach (var item in items.SearchQuery(query, pages))
                {
                    string url = item.Url;
                    myBot.audioManager.play(e, e.User, url);
                    Console.WriteLine(item.Title);
                    break;
                }
                */
            });
        }

        public string getVideoUrl(string searchTerm)
        {
            string html = this.webClient.DownloadString("https://www.youtube.com/results?search_query=" + searchTerm + "&page=1");
            string pattern = "<div class=\"yt-lockup-content\">.*?title=\"(?<NAME>.*?)\".*?</div></div></div></li>";
            MatchCollection result = Regex.Matches(html, pattern, RegexOptions.Singleline);
            string url = string.Concat("http://www.youtube.com/watch?v=", VideoItemHelper.cull(result[0].Value, "watch?v=", "\""));
            return url;
        }

        public override void onCommand(CommandEventArgs e, DiscordClient discord, string[] args)
        {
            e.Channel.SendMessage("Searching for first video...");
            string url = String.Join(" ", args);
            string videoUrl = getVideoUrl(url);
            myBot.audioManager.play(e, e.User, videoUrl);
        }
    }
}
