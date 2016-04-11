using System;

namespace Preference_Testing
{
    public class OpCommands : CommandList
    {
        private Action<Command> ChangeMode;

        public OpCommands(Action<string, Command, Connection.sendType> sendMessage,
            Action<Command> changeMode) : base(sendMessage)
        {
            ChangeMode = changeMode;

            Command["mode"] = new Action<Command>((c) => ChangeMode(c));
            Help["mode"] = "Set preferences for this channel. Usage: mode <list|preference> <on|off>";
        }
    }
}

