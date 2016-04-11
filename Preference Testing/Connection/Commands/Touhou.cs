using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace Preference_Testing
{
    public class Touhou : CommandList
    {
        public Touhou(Action<string, Command, Connection.sendType> sendMessage) : base(sendMessage)
        {
            Command["playing"] = new Action<Command>((c) => GetSongInfo(c));
            Help["playing"] = "Displays current song info from Gensokyo Radio";
        }

        private void GetSongInfo(Command command)
        {
            string xml = new WebClient().DownloadString("http://gensokyoradio.net/xml/");
            XmlSerializer serializer = new XmlSerializer(typeof(GENSOKYORADIODATA));
            GENSOKYORADIODATA data = null;
            StringReader reader = new StringReader(xml);

            data = (GENSOKYORADIODATA)serializer.Deserialize(reader);
            reader.Close();

            ss(string.Format("Now playing: {0} - {1} - {2} - {3}", data.SONGINFO.TITLE, data.SONGINFO.ARTIST, data.SONGINFO.ALBUM, data.SONGINFO.CIRCLE), command);
            
        }
    }
}

