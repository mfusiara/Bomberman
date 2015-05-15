using System.Collections.Generic;

namespace DataStorage.Serialization
{
    public interface IGameInfoSerializer
    {
        void Serialize(List<GameInfo> data);
        List<GameInfo> Deserialize();
    }

    public class GameInfoSerializer : Serializer<List<GameInfo>>, IGameInfoSerializer
    {
        public GameInfoSerializer(string fileName) : base(fileName)
        {
        }
    }
}