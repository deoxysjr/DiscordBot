﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Bot.Commands.AudioCommands.Playlists;

namespace Bot.Commands.AudioCommands
{
    class PlaylistList : BotCommand
    {

        MyBot myBot;

        public PlaylistList(MyBot myBot) : base("playlist list", "Load a playlist!", "!playlist list")
        {
            this.myBot = myBot;
        }

        public override void onCommand(CommandEventArgs e, DiscordClient discord, string[] args)
        {
            PlaylistManager playlistManager = new PlaylistManager();
            int i = 1;
            string complete = "```";
            foreach (string playList in playlistManager.listPlaylists())
            {
                Queue<YouTubeVideo> queue = playlistManager.getPlaylist(playList);
                complete += "\n" + i + ". " + playList + " (" + queue.Count + " songs)";
                i++;
            }
            e.Channel.SendMessage(complete + "```");
        }
    }
}
