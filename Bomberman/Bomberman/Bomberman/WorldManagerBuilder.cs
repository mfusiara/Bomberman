using Autofac;
using DataStorage;
using Domain;
using GameLibrary.WorldManagement;

namespace Bomberman
{
    public class WorldManagerBuilder
    {
        private readonly IServiceLocator _serviceLocator;

        public World World { get; set; }
        public WorldAssembler WorldAssembler { get; set; }
        public GameInfo GameInfo { get; set; }
        public Scoring Scoring { get; set; }
        public WorldManagerBuilder(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public WorldManager Build()
        {
            var worldManager = _serviceLocator.Container.Resolve<WorldManager>(
                new TypedParameter(typeof(World), World),
                new TypedParameter(typeof(WorldAssembler), WorldAssembler),
                new TypedParameter(typeof(GameInfo), GameInfo),
                new TypedParameter(typeof(Scoring), Scoring));

            return worldManager;
        }
    }
}