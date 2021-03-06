﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Bot.Audio;

namespace Bot.Commands.AudioCommands.Playlists
{
    class PlaylistManager
    {

        public PlaylistManager()
        {
            if (!Directory.Exists((@".\playlists")))
            {
                Directory.CreateDirectory(@".\playlists");
            }
        }

        public string[] listPlaylists()
        {
            DirectoryInfo d = new DirectoryInfo(@".\playlists");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.playlist"); //Getting Text files
            string[] result = new string[Files.Count()];
            int i = 0;
            foreach (FileInfo file in Files)
            {
                result[i] = file.Name.Replace(".playlist", "");
                i++;
            }
            return result;
        }

        public void savePlaylist(string name, Queue<YouTubeVideo> queue)
        {
            if (playListsExists(name)) return;

            using (System.IO.StreamWriter file =
         new System.IO.StreamWriter(@".\playlists\" + name + ".playlist"))
            {
                foreach (YouTubeVideo ytVideo in queue)
                {
                    file.WriteLine(ytVideo.toString());
                }
            }
        }

        public void loadPlaylist(string name, string requester, AudioManager audioManager)
        {
            if (!playListsExists(name) && !name.Contains("www.youtube"))
            {
                audioManager.sendMessage("No such playlist exists!");
            }

            audioManager.sendMessage("Loading playlist...");
            Queue<YouTubeVideo> queue = new Queue<YouTubeVideo>();

            if (name.Contains("www.youtube"))
            {
                var request = YouTubeVideo.auth().PlaylistItems.List("contentDetails");
                string id = name.Substring(name.IndexOf("list=") + 1).Replace("ist=", "");
                Console.WriteLine(id);
                request.PlaylistId = id;
                var response = request.Execute();

                YouTubeVideo[] videos = new YouTubeVideo[response.Items.Count];
                int i = 0;
                foreach (var item in response.Items)
                {
                    videos[i++] = new YouTubeVideo("http://www.youtube.com/watch?v=" + item.ContentDetails.VideoId, requester);
                }

                foreach (YouTubeVideo video in videos)
                {
                    queue.Enqueue(video);
                }
            }
            else {
                string[] lines = System.IO.File.ReadAllLines(@".\playlists\" + name + ".playlist");

                foreach (string line in lines)
                {
                    queue.Enqueue(YouTubeVideo.fromString(line));
                }
            }
            audioManager.setQueue(queue);
            audioManager.sendMessage("Playlist loaded!");
        }

        public void deletePlaylist(string name)
        {
            try
            {
                if (File.Exists(@".\playlists\" + name + ".playlist"))
                {
                    File.Delete(@".\playlists\" + name + ".playlist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while trying to delete the playlist!");
            }
        }

        public bool playListsExists(string name)
        {
            return File.Exists((@".\playlists\" + name + ".playlist"));
        }

        public Queue<YouTubeVideo> getPlaylist(string name)
        {
            if (!playListsExists(name))
            {
                Console.WriteLine("No playlist with that name!");
                return null;
            }

            Queue<YouTubeVideo> queue = new Queue<YouTubeVideo>();
            string[] lines = System.IO.File.ReadAllLines(@".\playlists\" + name + ".playlist");

            foreach (string line in lines)
            {
                queue.Enqueue(YouTubeVideo.fromString(line));
            }
            return queue;
        }
    }
}
