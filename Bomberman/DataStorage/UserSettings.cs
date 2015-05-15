using DataStorage.Enumerations;

namespace DataStorage
{
    public class UserSettings
    {
        public int UserId { get; set; }
        public bool Music { get; set; }
        public bool SFX { get; set; }
        public ControlType Control { get; set; }

        public UserSettings()
        {
            Music = true;
            SFX = true;
            Control = ControlType.ARROWS;
        }
    }
}