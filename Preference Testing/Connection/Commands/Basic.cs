using System;
using System.Collections.Generic;
using System.Linq;

namespace Preference_Testing
{
    public class Basic : CommandList
    {
        public Basic(Action<string, Command, Connection.sendType> sendMesage) : base(sendMesage)
        {
            Command["blah"] = new Action<Command>((c) => ss("Blah to you too, " + c.nick, c));
            Help["blah"] = "The first command put into the bot.";

            Command["about"] = new Action<Command>((c) => ss(
                    "I'm a simple bot put together by Googie2149. Later on I'll have a github link here.", c));
            Help["about"] = "Just use the command.";

            Help["help"] = "You get no help.";

            Inline["youtube"] = new Action<Command>((c) => ss("aaa", c));
        }
    }
}

