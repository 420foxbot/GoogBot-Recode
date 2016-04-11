/* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Preference_Testing
{
    [XmlRoot(ElementName="BITRATE")]
    public class BITRATE {
        [XmlElement(ElementName="BITRATE_1")]
        public string BITRATE_1 { get; set; }
    }

    [XmlRoot(ElementName="SERVERINFO")]
    public class SERVERINFO {
        [XmlElement(ElementName="LASTUPDATE")]
        public string LASTUPDATE { get; set; }
        [XmlElement(ElementName="SERVERS")]
        public string SERVERS { get; set; }
        [XmlElement(ElementName="STATUS")]
        public string STATUS { get; set; }
        [XmlElement(ElementName="LISTENERS")]
        public string LISTENERS { get; set; }
        [XmlElement(ElementName="BITRATE")]
        public BITRATE BITRATE { get; set; }
        [XmlElement(ElementName="MODE")]
        public string MODE { get; set; }
        [XmlElement(ElementName="AIMS")]
        public string AIMS { get; set; }
    }

    [XmlRoot(ElementName="SONGINFO")]
    public class SONGINFO {
        [XmlElement(ElementName="TITLE")]
        public string TITLE { get; set; }
        [XmlElement(ElementName="ARTIST")]
        public string ARTIST { get; set; }
        [XmlElement(ElementName="ALBUM")]
        public string ALBUM { get; set; }
        [XmlElement(ElementName="YEAR")]
        public string YEAR { get; set; }
        [XmlElement(ElementName="CIRCLE")]
        public string CIRCLE { get; set; }
    }

    [XmlRoot(ElementName="SONGTIMES")]
    public class SONGTIMES {
        [XmlElement(ElementName="DURATION")]
        public string DURATION { get; set; }
        [XmlElement(ElementName="PLAYED")]
        public string PLAYED { get; set; }
        [XmlElement(ElementName="REMAINING")]
        public string REMAINING { get; set; }
        [XmlElement(ElementName="SONGSTART")]
        public string SONGSTART { get; set; }
        [XmlElement(ElementName="SONGEND")]
        public string SONGEND { get; set; }
    }

    [XmlRoot(ElementName="MISC")]
    public class MISC {
        [XmlElement(ElementName="SONGID")]
        public string SONGID { get; set; }
        [XmlElement(ElementName="IDCERTAINTY")]
        public string IDCERTAINTY { get; set; }
        [XmlElement(ElementName="CIRCLELINK")]
        public string CIRCLELINK { get; set; }
        [XmlElement(ElementName="ALBUMART")]
        public string ALBUMART { get; set; }
        [XmlElement(ElementName="CIRCLEART")]
        public string CIRCLEART { get; set; }
        [XmlElement(ElementName="RATING")]
        public string RATING { get; set; }
        [XmlElement(ElementName="TIMESRATED")]
        public string TIMESRATED { get; set; }
        [XmlElement(ElementName="FORCEDELAY")]
        public string FORCEDELAY { get; set; }
    }

    [XmlRoot(ElementName="GENSOKYORADIODATA")]
    public class GENSOKYORADIODATA {
        [XmlElement(ElementName="SERVERINFO")]
        public SERVERINFO SERVERINFO { get; set; }
        [XmlElement(ElementName="SONGINFO")]
        public SONGINFO SONGINFO { get; set; }
        [XmlElement(ElementName="SONGTIMES")]
        public SONGTIMES SONGTIMES { get; set; }
        [XmlElement(ElementName="MISC")]
        public MISC MISC { get; set; }
    }

}