using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ChatSharp;

namespace Preference_Testing
{
    public partial class Connection
    {
        private Config LoadConfig(string configLocation)
        {
            bool configExists = System.IO.File.Exists(configLocation);
            if (configExists)
            {
                Console.WriteLine("Found your config file!");
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(configLocation));
            }
            else
            {
                Console.WriteLine("There isn't a configuration file in that location! Exiting...");
                Environment.Exit(0);

                return null; 

                /* Commented out because the config file changes consantly
                Console.WriteLine("Couldn't find you config file! Would you like to generate one now? y/n [y]");
                bool generate = getAnswer("y");

                if (generate)
                {
                    GenerateConfig(configLocation);
                }
                else
                {
                    Console.WriteLine("Well I have nothing to connect with. Jerk.");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                */
            }
        }

        private void SaveConfig(Config config, string configLocation)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(configLocation))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, config);
            }
        }
    }

    public class Config
    {
        public int sendDelay; // Not yet implemented
        public int port;
        public bool ssl;
        public bool ignoreInvalidSSL;
        public string Server = "";
        public string nick = "";
        public string realName = "";
        public string userName = "";
        public string password = ""; // TODO: Find a better way of storing passwords.
        public string quitMessage = "";
        public bool debug;

        public bool hostNameForAdmin; // This is for networks with set hostmasks, like Snoonet.
//        public List<User> adminList = new List<User>();
//        public List<User> ignoreList = new List<User>();
        public Dictionary<string, string> adminList = new Dictionary<string, string>();
        public Dictionary<string, string> ignoreList = new Dictionary<string, string>();
//        public List<Channel> channels = new List<Channel>();
        public Dictionary<string, Channel> channels = new Dictionary<string, Channel>();
    }

//    public class User
//    {
//        public string nick = "";
//        public string hostName = "";
//    }

    public class Channel
    {
        public string channelName;
        public bool checkReddit;
        public Dictionary<string, bool> subReddits = new Dictionary<string, bool>();
        public Dictionary<string, bool> channelPrefs = new Dictionary<string, bool>();

        public Channel()
        {   // Channel settings, for what commands are active
            // These have to match the description (prefExplain) list on Connection.cs
            // Sorry. If someone else has a better idea of how to get them to sync, let me know.
            channelPrefs["basic"] = true;
            channelPrefs["general"] = false; // General things, like youtube links and quote functionality
            channelPrefs["touhou"] = false;
            channelPrefs["meme"] = false;   // Annoying things, but some channels enjoy it.
            channelPrefs["actrade"] = false; // actrade is animalcrossing with a special 3DS friend code list
            channelPrefs["animalcrossing"] = false; // Animal Crossing for 3DS commands

            // Only one channel uses normal animalcrossing

            // New modes to put in: 
            // Discord (minmal general, have general inherit discord)
            // Touhou (...I have no ideas)
        }

        public bool? CheckPreference(string preference)
        {
            if (this.channelPrefs.ContainsKey(preference))
                return this.channelPrefs[preference];
            else
                return null;
        }

        // Returns null if the key isn't found or the setting is invalid
        // Returns true if the setting was changed
        // Returns false if that was already the setting
        public bool? SetPreference(string preference, bool? setting)
        {
            if (this.channelPrefs.ContainsKey(preference))
            {
                // Check if that's already the setting
                if (this.channelPrefs[preference] == setting)
                    return false;
                else
                {
                    this.channelPrefs[preference] = (bool)setting;
                    return true;
                }
            }
            else
                return null;
        }
    }


}