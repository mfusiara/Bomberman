using System;
using System.Collections.Generic;
using System.IO;
using DataStorage;
using DataStorage.Serialization;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    public class SerializerTest
    {
        [Test]
        public void Serialize()
        {
            var serializer = new Serializer<String>("Test_Serializer.xml");
            serializer.Serialize("Test");
        }

        [Test]
        public void Deserialize_File_Not_Exist()
        {
            const string filePath = "Test_Serialzier_Not_Exist_File.xml";
            if(File.Exists(filePath))
                File.Delete(filePath);

            var serializer = new Serializer<String>(filePath);
            var result = serializer.Deserialize();

            Assert.IsNull(result);
        }

        [Test]
        public void SettingsSerialzier()
        {
            var serializer = new UserSettingsSerializer("USS.xml");
            serializer.Serialize(new List<UserSettings>{new UserSettings{UserId = 1}});
        }

        [Test]
        public void SettingsSerialzier_Deserialzie()
        {
            var serializer = new UserSettingsSerializer("USS.xml");
            var usersSettings = serializer.Deserialize();
        }
    }
}