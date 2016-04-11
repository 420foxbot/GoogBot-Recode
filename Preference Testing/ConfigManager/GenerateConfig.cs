using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ChatSharp;
using Mono.Data.Sqlite;

namespace Preference_Testing
{
    public partial class ConfigManager
    {
        #region Generate Config

        #region Config Shortcuts
        private bool getAnswer()
        {
            return getAnswer("y", "n", "");
        }

        private bool getAnswer(string defaultAnswer)
        {
            return getAnswer("y", "n", defaultAnswer);
        }

        private bool getAnswer(string affirmative, string negative, string defaultAnswer)
        {
            string input = "";

            do
            {
                input = Console.ReadLine().ToLower();

                if (input == affirmative || input == negative)
                    break;
                else if (defaultAnswer != "" && input == "")
                    break;
                else
                    Console.WriteLine("Nope, gotta enter either {0} or {1}", affirmative, negative);
            } while (true);

            if (input == affirmative || defaultAnswer == affirmative)
                return true;
            else
                return false;
        }

        private int getNumber()
        {
            return getNumber(-1);
        }

        private int getNumber(int defaultNumber)
        {
            int temp;
            string input = "";
            do
            {
                input = Console.ReadLine();
                if (input == "" && defaultNumber != -1)
                    return defaultNumber;

                if (!int.TryParse(input, out temp))
                    Console.WriteLine("Not a valid number! Try again!");
                else
                    return temp;
            } while (true);
        }

        private string getString()
        {
            return getString("");
        }

        private string getString(string defaultString)
        {
            string input = "";
            do
            {
                input = Console.ReadLine();
                if (input != "")
                    return input;
                else if (defaultString != "")
                    return defaultString;
                else
                    Console.WriteLine("What am I supposed to do with this crap? It's just an empty string!");
            } while (true);
        }
        #endregion
        private void GenerateConfig(string configLocation)
        {
            loadedConfig = new Config();

            Console.WriteLine("What server would you like to connect to? (Just the hostname, not the port.)");
            loadedConfig.Server = getString();

            Console.WriteLine("[ ] <= means default option");
            Console.WriteLine("What port are you using? [6667]");
            loadedConfig.port = getNumber(6667);

            Console.WriteLine("Are you using SSL? y/n [n]");
            loadedConfig.ssl = getAnswer("n");

            Console.WriteLine("Ignore invalid SSL certificates? y/n [n]");
            loadedConfig.ignoreInvalidSSL = getAnswer("n");

            Console.WriteLine("What nickname would you like me to use? [GoogBot]");
            loadedConfig.nick = getString("GoogBot");

            Console.WriteLine("What's my username? [GoogBot]");
            loadedConfig.userName = getString("GoogBot");

            Console.WriteLine("Should I use a password to connect? y/n [n]");
            bool pass = getAnswer("n");

            if (pass)
            {
                Console.WriteLine("What's the password?");
                loadedConfig.password = getString();
            }

            Console.WriteLine("What should my real name be? [GoogBot]");
            loadedConfig.realName = getString("GoogBot");

            Console.WriteLine("What is my owner's nickname?");
            loadedConfig.ownerNick = getString();

            Console.WriteLine("Are there any users you would like to ignore entirely? y/n [n]");
            bool ignore = getAnswer("n");

            if (ignore)
            {
                string input = "";
                do
                {
                    input = getString();
                    if (input != ";")
                        loadedConfig.ignoredNicks.Add(input);
                } while (input != ";");
            }

            Console.WriteLine("Should I have a customized quit message? y/n [n]");
            bool message = getAnswer("n");

            if (message)
            {
                Console.WriteLine("What should it be?");
                loadedConfig.quitMessage = getString();
            }
            else
                loadedConfig.quitMessage = "I've been told to quit, bye!";

            Console.WriteLine("Would you like to add a channel now? You can always PM the bot after it connects. y/n [y]");
            bool makechan = getAnswer("y");

            if (makechan)
            {
                loadedConfig.channels.Add(createChannel());

                bool makeAnother = false;
                do
                {
                    Console.WriteLine("Would you like to add another channel now? y/n [n]");
                    makeAnother = getAnswer("n");
                } while (makeAnother);
            }

            SaveConfig(loadedConfig, configLocation);

            Console.WriteLine("Your configuration file has been saved!");

            Console.WriteLine("Enjoy your bot!");
        }

        private Channel createChannel()
        {
            Channel tempChannel = new Channel();

            Console.WriteLine("Name of the channel? Include the #");
            tempChannel.channelName = getString();

            Console.WriteLine("Does this channel have one or more subreddits associated with it? y/n [n]");
            tempChannel.checkReddit = getAnswer("n");

            if (tempChannel.checkReddit)
            {
                Console.WriteLine("What subreddits should I watch? Do not include the /r/. Type ; to stop.");
                string input;
                bool prefix;
                do
                {
                    input = "";
                    prefix = false;

                    input = getString();
                    if (input == ";")
                    {
                        if (tempChannel.subReddits.Count == 0)
                        {
                            Console.WriteLine("Oh, you didn't want to watch any subreddits? That's okay.");
                            tempChannel.checkReddit = false;
                        }

                        break;
                    }

                    Console.WriteLine("Should the subreddit name be displayed? Choose yes if you have more than one subreddit. y/n [y]");
                    prefix = getAnswer("y");

                    if (input != ";")
                        tempChannel.subReddits.Add(input, prefix);
                } while (input != ";");
            }

            Console.WriteLine("Do you want {0} to behave as an actrade channel? y/n [n]", tempChannel.channelName);
            Console.WriteLine("actrade is a customized version of the animalcrossing mode, with a specific FC database.");
            Console.WriteLine("The animalcrossing mode can provide information about villagers, moriDB item searches, and a local FC database.");
            tempChannel.channelPrefs["actrade"] = getAnswer("n");

            if (!tempChannel.channelPrefs["actrade"])
            {
                Console.WriteLine("Do you want an animalcrossing channel then? y/n [n]");
                tempChannel.channelPrefs["animalcrossing"] = getAnswer("n");
            }

            Console.WriteLine("Do you want {0} to behave as a spooky channel? y/n [n]", tempChannel.channelName);
            Console.WriteLine("This provides... well, stuff. It's silly. And annoying. Don't blame me for your users rioting.");
            tempChannel.channelPrefs["spooky"] = getAnswer("n");

            Console.WriteLine("Channel added!");

            return tempChannel;
        }

        #endregion
    }
}

