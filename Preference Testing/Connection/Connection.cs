using System;
using ChatSharp;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Preference_Testing
{
    public partial class Connection
    {
        private IrcClient client;
        private Config config;
        private string ConfigLocation;
        private Library CommandLibrary;
        private Dictionary<string, string> prefExplain = new Dictionary<string, string>();
        private Dictionary<string, string> prefDescrip = new Dictionary<string, string>();

        private void SetPreferenceExplanations()
        {
            prefExplain["basic"] = "Just enough to be useful.";
            prefDescrip["basic"] = "Basic";
            prefExplain["general"] = "Just a general usage mode. Doesn't do a whole lot yet.";
            prefDescrip["general"] = "General";
            prefExplain["touhou"] = "Some Touhou and Gensokyo Radio related commands.";
            prefDescrip["touhou"] = "Gensokyo Radio";
            prefExplain["meme"] = "Annoying things. You don't want this.";
            prefDescrip["meme"] = "Meme";
            prefExplain["actrade"] = "Animal Crossing commands and a distinct friend code list.";
            prefExplain["actrade"] = "Animal Crossing*";
            prefExplain["animalcrossing"] = "Animal Crossing commands.";
            prefExplain["animalcrossing"] = "Animal Crossing";
        }

        public Connection(string configLocation)
        {
            CommandLibrary = new Library(SendMessage, JoinChannel, PartChannel, ChangeChannelPreferences, StupidQuit);
            SetPreferenceExplanations();

            ConfigLocation = configLocation;
            config = LoadConfig(ConfigLocation);
            client = new IrcClient(string.Format("{0}:{1}", config.Server, config.port.ToString()), 
                new IrcUser(config.nick, config.userName, config.password, config.realName), 
                useSSL: config.ssl);
            client.IgnoreInvalidSSL = config.ignoreInvalidSSL;
            client.Settings.GenerateRandomNickIfRefused = true; // Might want to move this to the config

            EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.AutoReset);

            if (config.debug)
            {
                client.NetworkError += (s, e) => Console.WriteLine("Error: " + e.SocketError);
                client.RawMessageSent += (s, e) => Console.WriteLine(">> {0}", e.Message);
            }
            client.RawMessageRecieved += Client_RawMessageRecieved;

            client.ConnectionComplete += client_ConnectionComplete;
            client.PrivateMessageRecieved += Client_PrivateMessageRecieved;

//            client.ChannelListRecieved += Client_ChannelListRecieved;

            client.ConnectAsync();

            client.UserKicked += Client_UserKicked;

            wait.WaitOne();
        }

        private void Client_UserKicked (object sender, KickEventArgs e)
        {
            if (e.Kicked == client.User)
            {
                client.JoinChannel(e.Channel.Name);
            }
        }

        private void Client_RawMessageRecieved (object sender, ChatSharp.Events.RawMessageEventArgs e)
        {
            if (config.debug)
                Console.WriteLine("<< {0}", e.Message);

            if (e.Message.Contains("INVITE " + client.User.Nick))
            {
                JoinChannel(e.Message.Substring(e.Message.LastIndexOf(":") + 1));
            }
        }

//        private void Client_ChannelListRecieved (object sender, ChatSharp.Events.ChannelEventArgs e)
//        {
//            // Fully connected, do something here
//        }

        private void StupidQuit(Command command)
        {
            Quit();
        }

        private void Quit()
        {
            client.Quit(config.quitMessage.ToString());

            Console.WriteLine("Shutting down, saving my config would probably be a good idea.");
            SaveConfig(config, ConfigLocation);

            Console.WriteLine("Done! Have a nice day!");

            Environment.Exit(0);
        }

        public enum sendType
        {
            message, notice, action
        }

        private bool CheckForChannel(string channel)
        {
            return config.channels.ContainsKey(channel);
        }

        private void JoinChannel(string channel)
        {
            if (!CheckForChannel(channel))
            {
                config.channels[channel] = new Channel
                    {
                        channelName = channel,
                        checkReddit = false
                    };

                client.JoinChannel(channel);
            }
            else
                Console.WriteLine("==Tried to join an already joined channel!==");
            
//            client.JoinChannel(channel);
        }

        private void PartChannel(string channel, string reason)
        {
            if (CheckForChannel(channel))
            {
                config.channels.Remove(channel);

                client.PartChannel(channel, reason);
            }
            else
                Console.WriteLine("==Tried to leave a channel I'm not in!==");
        }

        private bool? StringToBool(string convert)
        {
            switch (convert)
            {
                case "1":
                    return true;
                case "true":
                    return true;
                case "on":
                    return true;
                case "yes":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "off":
                    return false;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        private void ChangeChannelPreferences(Command command)
        {
            if (command.channelMessage && CheckForChannel(command.channel.channelName))
            {
                // Not doing jack in a PM
                if (command.parameters.Length >= 2)
                {
                    // Changing a mode
                    bool? change = StringToBool(command.parameters[1]);

                    if (change != null)
                    {
                        bool? result = 
                            config.channels[command.channel.channelName].SetPreference(
                                command.parameters[0], change);

                        switch (result)
                        {
                            case true:
                                SendMessage("Preference saved!", command, sendType.message);
                                break;
                            case false:
                                SendMessage("That's already the set.", command, sendType.message);
                                break;
                            case null:
                                SendMessage("Possible modes to choose from: " +
                                    string.Join(", ", 
                                        prefExplain.Keys.ToArray()),
                                    command, sendType.message);
                                break;
                        }
                    }
                    else
                        SendMessage("Invalid setting! Try on or off.", command, sendType.message);
                }
                else if (command.firstParameter != "")
                {
                    // Checking the status of a mode and what it does
                    // ?mode list should work as well
                    if (command.firstParameter != "list")
                    {
                        bool? check = config.channels[command.channel.channelName].CheckPreference(command.firstParameter);

                        if (check != null)
                        {
                            string help = prefExplain[command.firstParameter];

                            if ((bool)check)
                            {
                                SendMessage(string.Format("Mode {0}: {1} | Value: On", command.firstParameter, help),
                                    command, sendType.message);
                            }
                            else
                            {
                                SendMessage(string.Format("Mode {0}: {1} | Value: Off", command.firstParameter, help),
                                    command, sendType.message);
                            }
                        }
                        else
                            SendMessage(string.Format("{0} isn't a valid mode!", 
                                    command.firstParameter), command, sendType.message);
                    }
                    else
                    {
                        SendMessage("All modes: " + string.Join(", ", prefExplain.Keys.ToArray()),
                            command, sendType.message);
                    }
                }
                else
                {
                    // Checking the current modes.
                    string output = "Enabled modes: ";
                    bool any = false;

                    foreach (string key in prefExplain.Keys)
                    {
                        if ((bool)config.channels[command.channel.channelName].CheckPreference(key))
                        {
                            if (any)
                                output += ", " + key;
                            else
                                output += key;

                            any = true; // If everything is disabled this won't run
                        }
                    }

                    if (any)
                        SendMessage(output, command, sendType.message);
                    else
                        SendMessage("No modes enabled on this channel.", command, sendType.message);
                }
            }
            else
                SendMessage("Sorry, you can't set a mode in PM.", 
                    command, sendType.message);

            /*
            if (command.parameters.Length >= 2)
            {
                if (CheckForChannel(command.channel.channelName))
                {
                    if (config.channels[command.channel.channelName]
                        .channelPrefs.ContainsKey(command.firstParameter))
                    {
                        if (StringToBool(command.parameters[1]) != null)
                        {
                            bool? result = 
                                config.channels[command.channel.channelName].SetPreference(
                                    command.parameters[0], StringToBool(command.parameters[1]));

                            switch (result)
                            {
                                case true:
                                    SendMessage("Preference saved!", command, sendType.message);
                                    break;
                                case false:
                                    SendMessage("That's already the set.", command, sendType.message);
                                    break;
                                case null:
                                    SendMessage("Possible modes to choose from: " +
                                        string.Join(", ", 
                                            config.channels[command.channel.channelName].channelPrefs.Keys.ToArray()),
                                        command, sendType.message);
                                    break;
                            }

                        }
                        else if (!command.channelMessage)
                            SendMessage("This is a PM. It's locked to general, sorry.",
                                command, sendType.message);
                        else
                            SendMessage("I don't seem to have that channel in my list.",
                                command, sendType.message);
                    }
                }
                else
                    SendMessage("Invalid setting! Try on or off.", command, sendType.message);
                    
            }
            */

        }

        private void SendMessage(string message, Command command, sendType type)
        {
            string destination;

            if (command.channelMessage)
                destination = command.channel.channelName;
            else
                destination = command.nick;

            switch (type)
            {
                case sendType.message:
                    client.SendMessage(message, destination);
                    break;
                case sendType.action:
                    client.SendAction(message, destination);
                    break;
                    // Implement notices once upgraded to the new version
            }
        }
    }
}

