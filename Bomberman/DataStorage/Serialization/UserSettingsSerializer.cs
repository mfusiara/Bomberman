using System.Collections.Generic;

namespace DataStorage.Serialization
{
    public interface IUserSettingsSerializer
    {
        void Serialize(List<UserSettings> data);
        List<UserSettings> Deserialize();
    }

    public class UserSettingsSerializer : Serializer<List<UserSettings>>, IUserSettingsSerializer
    {
        public UserSettingsSerializer(string fileName) : base(fileName)
        {
        }
    }
}