using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ChatSharp;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Preference_Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hi there! I'll be starting up in just a bit...");
            Connection connection = new Connection("Settings.json"); // TODO: Figure out command line arguments
        }
    }

    #region wtf
    /*

    class CommandLibrary
    {
        public enum channelType { actrade, animalcrossing, general, spooky };

        public class ChannelInformation
        {
            public string Name;
            public CommandLibrary.channelType ChannelType;
            public bool DisplaySubPrefix;
            public List<string> Subreddit;
            public bool spooky;

            public Dictionary<channelType, bool> Types = new Dictionary<channelType, bool>();

        }
    }

    class Connection
    {
        private Preferences LoadedPreferences = null;
        string settings = "Settings.xml";

        public Connection()
        {
            CommandLibrary.ChannelInformation channel = new CommandLibrary.ChannelInformation();

            channel.Name = "#test";
            channel.ChannelType = CommandLibrary.channelType.spooky;
            channel.DisplaySubPrefix = true;
            channel.Subreddit = new List<string>();
            channel.Subreddit.Add("subreddit");
            channel.spooky = false;
            channel.Types[CommandLibrary.channelType.spooky] = false;
            channel.Types[CommandLibrary.channelType.actrade] = true;
            channel.Types[CommandLibrary.channelType.animalcrossing] = false;
            channel.Types[CommandLibrary.channelType.general] = true;

            LoadedPreferences = new Preferences();
            LoadedPreferences.Channels = new List<CommandLibrary.ChannelInformation>();
            LoadedPreferences.Channels.Add(channel);
            LoadedPreferences.Nicks = new List<string>();
            LoadedPreferences.Nicks.Add("Nick1");
            LoadedPreferences.Nicks.Add("Nick2");
            LoadedPreferences.OwnerIdent = "Owner";
            LoadedPreferences.Password = "Pass";
            LoadedPreferences.Port = 1234;
            LoadedPreferences.Prefix = "prefix";
            LoadedPreferences.RealName = "nane";
            LoadedPreferences.SendDelay = 200;
            LoadedPreferences.Server = "server";
            LoadedPreferences.UserName = "user";
            LoadedPreferences.ZNC = false;


            SavePreferences(LoadedPreferences);

            Console.ReadLine();
        }

        private void LoadPreferences()
        {
            Console.WriteLine("Loading preferences...");

            //            Console.WriteLine("Application directory: " + Path.GetDirectoryName());
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());

            //            string[] files = Directory.GetFiles(@"./");
            //            Console.WriteLine("FILES:");
            //            foreach (string i in files)
            //            {
            //                Console.WriteLine(i);
            //            }
            // Check if settings exist. If not, create them.
            if (File.Exists(settings))
            {
                try
                {
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(Preferences));

                    FileStream ReadFileStream = new FileStream(settings, FileMode.Open, FileAccess.Read, FileShare.Read);
                    LoadedPreferences = (Preferences)SerializerObj.Deserialize(ReadFileStream);
                    ReadFileStream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong reading the preferences file!");
                    Console.WriteLine("Message: " + e.Message);
                    Console.WriteLine("Exception: " + e.StackTrace);

                    System.Environment.Exit(1);
                }
            }
            else
            {
                // TODO: Make a function that will allow the user to create a file
                //                CreatePreferences();
                Console.WriteLine("Settings.xml not found!");
                System.Environment.Exit(1);
            }
        }

        private void SavePreferences(Preferences savethis)
        {
            // Build preferences file
            //Preferences tempPref = new Preferences();

            ////            foreach (string channel in channels)
            ////            {
            ////                tempPref.Channels.Add(channel);
            ////            }
            //tempPref.Channels = Channels;
            //tempPref.Nicks.Add("Nick1");
            //tempPref.Nicks.Add("Nick2");
            //tempPref.Password = LoadedPreferences.Password;
            //tempPref.RealName = LoadedPreferences.RealName;
            //tempPref.Server = LoadedPreferences.Server;
            //tempPref.UserName = LoadedPreferences.UserName;
            //tempPref.OwnerIdent = OwnerIdent;
            //tempPref.Prefix = LoadedPreferences.Prefix;
            //tempPref.Port = LoadedPreferences.Port;
            //tempPref.SendDelay = LoadedPreferences.SendDelay;
            //tempPref.ZNC = LoadedPreferences.ZNC;

            // Write it to disk
            try
            {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(Preferences));

                TextWriter WriteFileStream = new StreamWriter(settings);
                SerializerObj.Serialize(WriteFileStream, savethis);
                WriteFileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong when saving preferences!");
                Console.WriteLine("Message: " + e.Message);
                Console.WriteLine("Exception: " + e.StackTrace);
            }
        }
    }

    [Serializable()]
    class Preferences
    {
        public int SendDelay;
        public bool ZNC;
        public string Server;
        public int Port;
        public List<string> Nicks = new List<string>();
        public string RealName;
        public string UserName;
        public string OwnerIdent;
        public string Prefix;
        public string Password;
        public List<CommandLibrary.ChannelInformation> Channels = new List<CommandLibrary.ChannelInformation>();
    } 

     */

    #endregion
}
