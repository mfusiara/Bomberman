using System.Xml.Serialization;

namespace Domain.DTO
{
    [XmlInclude(typeof(KangarooDTO))]
    [XmlInclude(typeof(KoalaDTO))]
    [XmlInclude(typeof(PlatypusDTO))]
    [XmlInclude(typeof(TaipanDTO))]
    [XmlInclude(typeof(WombatDTO))]
    public class EnemyDTO : WorldElementDTO
    {
        public int Hitpoints { get; set; }

    }

    public class KangarooDTO : EnemyDTO
    {
        
    }

    public class KoalaDTO : EnemyDTO
    {

    }

    public class PlatypusDTO : EnemyDTO
    {

    }

    public class TaipanDTO : EnemyDTO
    {

    }

    public class WombatDTO : EnemyDTO
    {

    }
}