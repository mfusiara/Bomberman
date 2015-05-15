using System.IO;
using System.Xml.Serialization;

namespace DataStorage.Serialization
{
    public interface ISerializer<T> where T : class 
    {
        void Serialize(T data);
        T Deserialize();
    }

    public class Serializer<T> : ISerializer<T> where T : class
    {
        private readonly string _fileName;

        public Serializer(string fileName)
        {
            _fileName = fileName;
        }

        public void Serialize(T data)
        {
            if(File.Exists(_fileName)) File.Delete(_fileName);
            var serializer = new XmlSerializer(typeof (T));
            using (var stream = File.OpenWrite(_fileName))
            {
                serializer.Serialize(stream, data);
            }
        }

        public T Deserialize()
        {
            try
            {
                T data;
                var serializer = new XmlSerializer(typeof (T));
                using (var stream = File.OpenRead(_fileName))
                {
                    data = (T) (serializer.Deserialize(stream));
                }
                return data;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
        }
    }
}