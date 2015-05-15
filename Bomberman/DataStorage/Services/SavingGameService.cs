using System.Collections.Generic;
using System.Linq;
using DataStorage.Serialization;
using Domain;
using Domain.Assemblers;

namespace DataStorage.Services
{
    public interface ISavingGameService
    {
        void Save(World world, GameInfo gameInfo, string name);
    }

    public class SavingGameService : ISavingGameService
    {
        private readonly IGameInfoSerializer _gameInfoSerializer;
        private readonly IDTOAssembler _dtoAssembler;

        public SavingGameService(IGameInfoSerializer gameInfoSerializer, IDTOAssembler dtoAssembler)
        {
            _gameInfoSerializer = gameInfoSerializer;
            _dtoAssembler = dtoAssembler;
        }

        public void Save(World world, GameInfo gameInfo, string name)
        {
            gameInfo.Name = name;
            gameInfo.World = _dtoAssembler.GetDTO(world);

            var gameInfos = _gameInfoSerializer.Deserialize() ?? new List<GameInfo>();
            int oldGameInfoIndex = gameInfos.FindIndex(gi => gi.Name == name);
            if (oldGameInfoIndex != -1) gameInfos[oldGameInfoIndex] = gameInfo;
            else gameInfos.Add(gameInfo);
            _gameInfoSerializer.Serialize(gameInfos);
        }
    }
}