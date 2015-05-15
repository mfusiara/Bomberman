using System.Xml.Serialization;

namespace DataStorage
{
    public class BestScore
    {
        public int UserId { get; set; }
        public int Score { get; set; }

        [XmlIgnore]
        public string UserName { get; set; }
    }
}