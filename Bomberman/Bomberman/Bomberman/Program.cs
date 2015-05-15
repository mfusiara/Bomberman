using Autofac;
using Bomberman.GameScreens;

namespace Bomberman
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var container = ServiceLocator.Instance.Container;

            using (var game = container.Resolve<Game1>())
            {
                var gameStateManager = container.Resolve<IGameStateManager>();
                game.Components.Add(gameStateManager);
                gameStateManager.ChangeState(ScreenType.Login);
                game.Run();
            }
        }
    }
#endif
}

