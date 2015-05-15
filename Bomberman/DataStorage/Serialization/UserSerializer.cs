using System.Collections.Generic;

namespace DataStorage.Serialization
{
    public interface IUserSerializer
    {
        void Serialize(List<User> users);
        List<User> Deserialize();
    }

    public class UserSerializer : Serializer<List<User>>, IUserSerializer
    {
        public UserSerializer(string fileName) : base(fileName)
        {
            
        }
    }
}