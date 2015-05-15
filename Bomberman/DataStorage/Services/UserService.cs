using System;
using System.Collections.Generic;
using System.Linq;
using DataStorage.Serialization;

namespace DataStorage.Services
{
    public interface ILoginService
    {
        User Login(String login);
        UserSettings GetUserSettings(int id);
    }

    public interface IUserFinder
    {
        User Find(int userId);
    }

    public interface ISettingsUpdater
    {
        void SaveSettings(UserSettings newSettings);
    }

    public class UserService : ILoginService, IUserFinder, ISettingsUpdater
    {
        private readonly IUserSerializer _userSerializer;
        private readonly IUserSettingsSerializer _userSettingsSerializer;

        public UserService(IUserSerializer userSerializer, IUserSettingsSerializer userSettingsSerializer)
        {
            _userSerializer = userSerializer;
            _userSettingsSerializer = userSettingsSerializer;
        }

        public User Login(String login)
        {
            var users = _userSerializer.Deserialize() ?? new List<User>();

            var user = users.FirstOrDefault(u => u.Name == login);
            if (user != null) return user;


            user = CreateUser(login);
            users.Add(user);

            _userSerializer.Serialize(users);
            
            return user;
        }

        public User Find(int userId)
        {
            var users = _userSerializer.Deserialize() ?? new List<User>();
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public UserSettings GetUserSettings(int id)
        {
            var users = _userSerializer.Deserialize();

            if (users == null || users.All(u => u.Id != id)) return null;
            var usersSettings = _userSettingsSerializer.Deserialize() ?? new List<UserSettings>();
            var userSettings = usersSettings.FirstOrDefault(u => u.UserId == id);

            if (userSettings == null)
            {
                userSettings = new UserSettings { UserId = id,};
                usersSettings.Add(userSettings);
                _userSettingsSerializer.Serialize(usersSettings);
            }
            
            return userSettings;
        }

        public void SaveSettings(UserSettings newSettings)
        {
            var usersSettings = _userSettingsSerializer.Deserialize();
            var userSettings = usersSettings.First(u => u.UserId == newSettings.UserId);
            userSettings.Music = newSettings.Music;
            userSettings.SFX = newSettings.SFX;
            userSettings.Control = newSettings.Control;
            _userSettingsSerializer.Serialize(usersSettings);
        }

        private User CreateUser(string login)
        {
            var user = new User
            {
                Name = login,
                Id = GenerateId(),
            };

            return user;
        }

        private int GenerateId()
        {
            var users = _userSerializer.Deserialize() ?? new List<User>();

            for (int i = 0; i < int.MaxValue; i++)
                if (users.All(u => u.Id != i)) return i;

            return -1;
        }
    }
}