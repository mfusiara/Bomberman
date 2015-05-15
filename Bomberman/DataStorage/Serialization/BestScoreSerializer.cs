using System.Collections.Generic;

namespace DataStorage.Serialization
{
    public interface IBestScoreSerializer
    {
        void Serialize(List<BestScore> data);
        List<BestScore> Deserialize();
    }

    public class BestScoreSerializer : Serializer<List<BestScore>>, IBestScoreSerializer
    {
        public BestScoreSerializer(string fileName) : base(fileName)
        {
        }
    }
}