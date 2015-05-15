using System.IO;
using DataStorage.Serialization;
using DataStorage.Services;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    public class UserServiceTest
    {
        private const string UsersFile = "Test_Users.xml";
        private const string SettingsFile = "Test_Settings.xml";
        private UserService _userService;

        [TestFixtureSetUp]
        public void Init()
        {
            if(File.Exists(UsersFile)) File.Delete(UsersFile);
            if(File.Exists(SettingsFile)) File.Delete(SettingsFile);
            _userService = new UserService(new UserSerializer(UsersFile), new UserSettingsSerializer(SettingsFile));
        }

        [Test]
        public void Login_New_User()
        {
            var user = _userService.Login("marcin");

            Assert.IsNotNull(user);
            Assert.IsTrue(user.Id >= 0);
            Assert.IsNotNull(File.Exists(UsersFile));
        }

        [Test]
        public void GetConfigFile_User_Not_Exist()
        {
            var config = _userService.GetUserSettings(1);

            Assert.IsNull(config);
        }

        [Test]
        public void GetConfigFile_User_Exist()
        {
            Login_New_User();
            var userId = 0;
            var config = _userService.GetUserSettings(userId);

            Assert.IsNotNull(config);
            Assert.IsTrue(config.UserId == userId);
        }
    }
}