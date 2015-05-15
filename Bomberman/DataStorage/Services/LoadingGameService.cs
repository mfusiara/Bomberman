using System.Collections.Generic;
using System.Linq;
using DataStorage.Serialization;

namespace DataStorage.Services
{
    public interface ILoadingGameService
    {
        IEnumerable<GameInfo> GetSavedGames(User user);
    }

    public class LoadingGameService : ILoadingGameService
    {
        private readonly IGameInfoSerializer _gameInfoSerializer;

        public LoadingGameService(IGameInfoSerializer gameInfoSerializer)
        {
            _gameInfoSerializer = gameInfoSerializer;
        }

        public IEnumerable<GameInfo> GetSavedGames(User user)
        {
            var result = _gameInfoSerializer.Deserialize();
            if (result == null) return new List<GameInfo>();
            return result.Where(info => info.UserId == user.Id);
        }  
    }
}