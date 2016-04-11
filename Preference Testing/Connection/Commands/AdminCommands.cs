using System;
using System.Collections.Generic;
using System.Linq;

namespace Preference_Testing
{
    public class AdminCommands : CommandList
    {   
        private Action<string> JoinChannel;
        private Action<string, string> PartChannel;
        private Action<Command> Quit;

        public AdminCommands(
            Action<string, Command, Connection.sendType> sendMesage,
            Action<string> joinChannel,
            Action<string, string> partChannel,
            Action<Command> quit) : base (sendMesage)
        {
            JoinChannel = joinChannel;
            PartChannel = partChannel;
            Quit = quit;

            Command["join"] = new Action<Command>((c) => JoinChannel(c.firstParameter));
            Help["join"] = "Make the bot join a channel. Usage: join <channel>";

            Command["part"] = new Action<Command>((c) => CheckPart(c));
            Help["part"] = "Make the bot part a channel. Usage: part <channel>";

            Command["test"] = new Action<Command>((c) => ss("Success!", (c)));
            Help["test"] = "Hey this works!";

            Command["quit"] = new Action<Command>((c) => Quit(c));
            Help["quit"] = "Disconnects and saves the configuration.";
        }

        private void CheckPart(Command command)
        {
            string reason = "Leaving";

            if (command.parameters.Length >= 2 && command.parameters[1] != "")
            {
                reason = command.parameters[1];
            }

            PartChannel(command.firstParameter, reason);
        }
    }
}

