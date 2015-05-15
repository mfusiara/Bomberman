using System;
using System.IO;
using System.Xml.Serialization;
using Domain.DTO;

namespace Domain.Serialization
{
    public interface IWorldSerializer
    {
        void Serialize(WorldDTO world, String path = "World.xml");
        WorldDTO Deserialize(String path = "World.xml");
    }

    public class WorldSerializer : IWorldSerializer
    {
        public void Serialize(WorldDTO world, String path = "World.xml")
        {
            var writer = new XmlSerializer(world.GetType());

            var file = new StreamWriter(path);
            writer.Serialize(file, world);
            file.Close();

        }

        public WorldDTO Deserialize(String path = "World.xml")
        {
            var x = new XmlSerializer(typeof(WorldDTO));
            return (WorldDTO)x.Deserialize(new StreamReader(path));
        }
    }
}