using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using ChatSharp;

namespace Preference_Testing
{
    public partial class Connection
    {
        private void Interpreter(ChatSharp.Events.PrivateMessageEventArgs e)
        {
//            Console.WriteLine("Interpreting...");
            Command tempCommand = new Command();
            if (!CheckIgnore(e.PrivateMessage.User.Nick, e.PrivateMessage.User.Hostname))
            {
                tempCommand.channelMessage = e.PrivateMessage.IsChannelMessage;
                if (e.PrivateMessage.IsChannelMessage)
                {
                    tempCommand.chanOp = CheckMode(e.PrivateMessage.User.ChannelModes[client.Channels[e.PrivateMessage.Source]].ToString());
                    tempCommand.channel = config.channels[e.PrivateMessage.Source];
                }
                tempCommand.source = e.PrivateMessage.Source;
//            Console.WriteLine(e.PrivateMessage.User.Nick);
//            Console.WriteLine(e.PrivateMessage.User.Hostname);
//            tempCommand.user.nick = e.PrivateMessage.User.Nick.ToString();
//            tempCommand.user.hostName = e.PrivateMessage.User.Hostname;
                tempCommand.nick = e.PrivateMessage.User.Nick;
                tempCommand.message = e.PrivateMessage.Message;
                tempCommand.hostName = e.PrivateMessage.User.Hostname;
                tempCommand.admin = CheckAdmin(tempCommand.nick, tempCommand.hostName);

//            Console.WriteLine("blah");
                tempCommand = parseCommand(tempCommand, e.PrivateMessage.Message);
//            Console.WriteLine("Success?");
                if (tempCommand.commandFound)
                {
                    ThreadStart processTaskThread = delegate
                    {
                        ExecuteCommand(tempCommand);
                    }; 
                    new Thread(processTaskThread).Start();
                }
                else
                {
                    foreach (string key in prefExplain.Keys)
                    {
                        if (CommandLibrary.DyanmicCheckInline(key, tempCommand))
                        {
                            ThreadStart processTaskThread = delegate
                                {
                                    ExecuteCommand(tempCommand);
                                }; 
                            new Thread(processTaskThread).Start();
                        }
                    }
                }

            }
            // General stuff down here, like 'doot' or youtube commands
            // Actually no, find a better way or something
        }

        private Command parseCommand(Command command, string message)
        {

            if (message.Length > 0)
            {

                #region Separate command from parameters
                // Check if command starts with ! or ?
                // Might drop the ! check
                // Also convert ToChar because reasons
                if (message[0] == Convert.ToChar("?") || 
                    message[0] == Convert.ToChar("!") || 
                    message[0] == Convert.ToChar(".") ||
                    message[0] == Convert.ToChar("/") ||
                    message[0] == Convert.ToChar(";"))
                {
                    string tempCommand = "";

                    // Now that it's been confirmed as a valid command, remove the first character! Also, trim it. Because extra spaces might screw with stuff
                    tempCommand = message.Substring(1).TrimEnd();
                    command.commandFound = true;

                    // Create a place for the parameters (if any) to be stored
                    string[] parameters = new string[] { "" };

                    // Check if there are parameters
                    if (message.Contains(" "))
                    {
                        // Make a new string array, and start to mess with it after removing the actual command part
                        parameters = tempCommand.Substring(
                            // This is used to remove the command part. It checks how long the command is (notice how similar it is to the code below)
                            Truncate(tempCommand, tempCommand.IndexOf(" ")).Length)
                            // Then this removes whitespace and will split the parameters up really nicely. Just like that...
                            // Last time I try and code without first looking at my older code. I can't believe I forgot about split
                            .Trim().Split(' ');

                        // Check if any of the cells are empty
                        bool empty = false;

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i] == "")
                            {
                                empty = true;
                                break;
                            }
                        }

                        if (empty)
                        {
                            // There's empty spaces! Screw that!
                            List<string> tempList = new List<string>();

                            // Take each entry that ISN'T dumb and put it into a list
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (parameters[i] != "")
                                {
                                    tempList.Add(parameters[i]);
                                }
                            }

                            // Now add it back to parameters
                            parameters = tempList.ToArray();
                        }

                        // Remove parameters, leaving command as just the command and also making it lowercase
                        tempCommand = Truncate(tempCommand, tempCommand.IndexOf(" "));
                    }

                    // Add the command to the stringbuilder
                    //                    output.AppendFormat("\nCommand:\n     {0}\n", command);
                    //
                    //                    if (parameters.Length > 0)
                    //                    {
                    //                        output.AppendLine("Parameters");
                    //
                    //                        for (int i = 0; i < parameters.Length; i++)
                    //                        {
                    //                            // Add each parameter with leading space so it isn't all bunched up, but add quotes so I can check for spaces within the thing
                    //                            // Also make sure it's not empty...
                    //                            if (parameters[i] != "")
                    //                                output.AppendLine(string.Format("{0}:  {1}", i.ToString("D2"), parameters[i]));
                    //                        }
                    //                    }
                    #endregion

                    command.command = tempCommand.ToLower();
                    command.firstParameter = parameters[0].ToLower();
                    command.parameters = parameters;
                }
            }

            return command;
        }

        private bool CheckAdmin(string nick, string hostName)
        {
            if (config.adminList.ContainsKey(nick))
            {
                if (config.hostNameForAdmin)
                {
                    if (config.adminList.ContainsValue(hostName))
                        return true;
                    else
                        return false;
                }
                else
                    return true;
                    
            }
            else if (config.adminList.ContainsValue(hostName))
                return true;
            else
                return false;
        }

        private bool CheckIgnore(string nick, string hostName)
        {
            if (config.ignoreList.ContainsKey(nick))
                return true;
            else if (config.ignoreList.ContainsValue(hostName))
                return true;
            else
                return false;
        }

        private bool CheckMode(string mode)
        {
            switch (mode)
            {
                case "o":
                    return true;
                case "q":
                    return true;
                case "s":
                    return true;
                case "h":
                    return true;
                default:
                    return false;
            }
        }

        private string Help(string parameter, Dictionary<string, string> dictionary)
        {
            if (parameter == "")
            {
                return "All possible commands: " + string.Join(", ", dictionary.Keys.ToArray());
            }

            string test = "";
            dictionary.TryGetValue(parameter, out test);
            return test;
        }

        // found here: http://www.dotnetperls.com/truncate
        // Truncates the command. Removes everything after a specificed number of characters
        private string Truncate(string source,  int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }

    public class Command
    {
        public string message = "";
        public string command = "";
        public string firstParameter = "";
        public string[] parameters = new string[] { "" };
//        public User user = new User();
        public string nick;
        public string hostName;
        public bool chanOp;
        public bool admin;
        public bool channelMessage;
        public bool commandFound = false;
        public string source;
        public Channel channel = new Channel();
    }
}

