using System;
using System.Collections.Generic;
using System.Linq;

namespace Preference_Testing
{
    public class Library
    {
        public AdminCommands admin;
        public OpCommands op;
        public Basic basic;
        public Touhou touhou;

        public Library(
            Action<string, Command, Connection.sendType> sendMessage,
            Action<string> joinChannel,
            Action<string, string> partChannel,
            Action<Command> changeMode,
            Action<Command> quit)
        {
            admin = new AdminCommands(sendMessage, joinChannel, partChannel, quit);
            op = new OpCommands(sendMessage, changeMode);
            basic = new Basic(sendMessage);
            touhou = new Touhou(sendMessage);
        }

        public bool DynamicRun(string key, Command command)
        {
            switch (key)
            {
                case "basic":
                    return basic.Run(command);
                case "touhou":
                    return touhou.Run(command);
                default:
                    return false;
            }
        }

        public string DynamicListHelp(string key, string parameter)
        {
            switch (key)
            {
                case "basic":
                    return basic.ListHelp(parameter);
                case "touhou":
                    return touhou.ListHelp(parameter);
                default:
                    return "";
            }
        }

        public string DynamicGetDescription(string key, string parameter)
        {
            switch (key)
            {
                case "basic":
                    return basic.GetDescription(parameter);
                case "touhou":
                    return touhou.GetDescription(parameter);
                default:
                    return "";
            }
        }

        public void DynamicRunInline(string key, Command command)
        {
            switch (key)
            {
                case "basic":
                    basic.RunInline(command);
                    break;
            }
        }

        public bool DyanmicCheckInline(string key, Command command)
        {
            switch (key)
            {
                case "basic":
                    return basic.CheckInline(command);
                default:
                    return false;
            }
        }
    }

    public abstract class CommandList
    {
        protected Dictionary<string, Action<Command>> Command = new Dictionary<string, Action<Command>>();
        protected Dictionary<string, string> Help = new Dictionary<string, string>();
        protected Dictionary<string, Action<Command>> Inline = new Dictionary<string, Action<Command>>();

        protected Action<string, Command, Connection.sendType> SendMessage;

        // ss = SimpleSend, for the small commands
        protected void ss(string message, Command command)
        {
            SendMessage(message, command, Connection.sendType.message);
        }

        public CommandList(
            Action<string, Command, Connection.sendType> sendMessage)
        {
            SendMessage = sendMessage;
        }

        public string ListHelp(string parameter)
        {
            return string.Join(", ", Help.Keys.ToArray());
        }

        public string GetDescription(string parameter)
        {
            if (Help.ContainsKey(parameter))
            {
                // Check that this is the list that has that command
                return Help[parameter];
            }
            else
                return ""; // Command not found.
        }

        public bool CheckInline(Command command)
        {
            if (Inline != null && Inline.Count > 0)
            {
                foreach (string key in Inline.Keys)
                {
                    if (command.message.Contains(key))
                        return true;
                    else
                        return false;
                }

                return false;
            }
            else
                return false;
        }

        public void RunInline(Command command)
        {
            if (Inline != null && Inline.Count > 0)
            {
                foreach (string key in Inline.Keys)
                {
                    if (command.message.Contains(key))
                        Inline[key].DynamicInvoke(command);
                }
            }
        }

        public bool Run(Command command)
        {
            if (Command.ContainsKey(command.command))
            {
                Command[command.command].DynamicInvoke(command);

                // Since there's multiple modes, you want to return help for each of them if they're enabled
                // This is getting too complicated dang it
                if (command.command == "help" && command.firstParameter == "")
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
    }
}

