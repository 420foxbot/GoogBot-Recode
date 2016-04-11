using System;
using ChatSharp;

namespace Preference_Testing
{
    public partial class Connection
    {

        private void Client_PrivateMessageRecieved (object sender, ChatSharp.Events.PrivateMessageEventArgs e)
        {
            if (e.PrivateMessage.IsChannelMessage)
            {
                Console.WriteLine("{0}: <{1}> {2}", e.PrivateMessage.Source, e.PrivateMessage.User.Nick, e.PrivateMessage.Message);
            }
            else
            {
                Console.WriteLine("*{0}: <{1}> {2}", e.PrivateMessage.Source, e.PrivateMessage.User.Nick, e.PrivateMessage.Message);

            }

            /*
            if (e.PrivateMessage.Message.ToLower().Contains("list"))
            {


                foreach (IrcChannel c in client.Channels)
                {
                    Console.WriteLine(c.Name);
                }
            }
            */

            Interpreter(e);
        }

        private void client_ConnectionComplete(object sender, EventArgs e)
        {
            // Set +B on ourselves, because we're a bot
            client.ChangeMode(client.User.Nick, "+B");
            Console.WriteLine("Connected!");

            // We need to log in here
//            client.SendMessage("IDENTIFY " + config.password, "NickServ");

            foreach (Channel channel in config.channels.Values)
            {
                client.JoinChannel(channel.channelName);
            }
        }
    }
}

