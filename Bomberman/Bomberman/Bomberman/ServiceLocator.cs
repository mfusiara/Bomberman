using System;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using Bomberman.GameScreens;
using Bomberman.Logging;
using Bomberman.Music;
using DataStorage.Serialization;
using DataStorage.Services;
using Domain.Assemblers;
using Domain.Enemies.Factories;
using Domain.Serialization;
using GameLibrary.Input;
using GameLibrary.WorldManagement;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    public interface IServiceLocator
    {
        IContainer Container { get; set; }
        void Init();
    }

    public class ServiceLocator : IServiceLocator
    {
        private bool _initialized;
        private IContainer _container;
        private static ServiceLocator _instance;

        public static ServiceLocator Instance
        {
            get { return _instance ?? (_instance = new ServiceLocator()); }
            set { _instance = value; }
        }

        public IContainer Container
        {
            get
            {
                if(_container == null) Init();
                return _container;
            }
            set { _container = value; }
        }

        public void Init()
        {
            if(_initialized)
                throw new Exception("ServiceLocator has been initalized");
            var builder = new ContainerBuilder();
            builder.RegisterType<Game1>().As<Game>().AsSelf().SingleInstance();
            builder.RegisterType<ScreenFactory>().As<IScreenFactory>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<GameStateManager>().As<IGameStateManager>().SingleInstance();
            builder.RegisterType<KeyActionsManager>().As<IKeyActionsManager>().SingleInstance();
            builder.RegisterType<UserService>().As<ILoginService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<UserService>().As<IUserFinder>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<UserService>().As<ISettingsUpdater>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterInstance(new UserSerializer(Settings.Default.UsersPath)).As<IUserSerializer>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterInstance(new UserSettingsSerializer(Settings.Default.UsersSettings))
                .As<IUserSettingsSerializer>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterInstance(new GameInfoSerializer("GameInfos.xml")).As<IGameInfoSerializer>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterInstance(new BestScoreSerializer("BestScores.xml")).As<IBestScoreSerializer>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<DTOAssembler>().As<IDTOAssembler>().SingleInstance()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<SavingGameService>().As<ISavingGameService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof (LoggingAspect));
            builder.RegisterType<BestScoresService>().As<IBestScoresService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<MusicManager>().As<IMusicManager>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<WorldSerializer>().As<IWorldSerializer>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<EnemyFactory>().As<IEnemyFactory>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterInstance(new Scoring()).As<IScoring>().SingleInstance()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<WorldManager>().AsSelf();
            builder.RegisterType<WorldBuilder>().WithParameter(new TypedParameter(typeof(string), @"Content\Worlds\World1.xml"))
                .As<IWorldBuilder>().SingleInstance()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<LoadingGameService>().As<ILoadingGameService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<LoginScreen>().Keyed<GameState>(ScreenType.Login)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<LoadGameScreen>().Keyed<GameState>(ScreenType.LoadGame)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<BestScoresScreen>().Keyed<GameState>(ScreenType.BestScores)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<HelpScreen>().Keyed<GameState>(ScreenType.Help)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<MainMenuScreen>().Keyed<GameState>(ScreenType.MainMenu)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<PauseScreen>().Keyed<GameState>(ScreenType.Pause)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<SaveGameScreen>().Keyed<GameState>(ScreenType.SaveGame)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<SettingsScreen>().Keyed<GameState>(ScreenType.Settings)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<GamePlayScreen>().Keyed<GameState>(ScreenType.GamePlay)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<GameOverScreen>().Keyed<GameState>(ScreenType.GameOver)
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterType<WorldManagerBuilder>().AsSelf()
                .EnableClassInterceptors().InterceptedBy(typeof(LoggingAspect));
            builder.RegisterInstance(this).As<IServiceLocator>();
            builder.RegisterType<LoggingAspect>();
            Container = builder.Build();

            _initialized = true;
        }
    }
}