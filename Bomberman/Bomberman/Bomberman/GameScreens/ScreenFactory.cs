using Autofac.Features.Indexed;

namespace Bomberman.GameScreens
{
    public interface IScreenFactory
    {
        GameState GetScreen(ScreenType screenType);
    }

    public class ScreenFactory : IScreenFactory
    {
        private readonly IIndex<ScreenType, GameState> _screens;

        public ScreenFactory(IIndex<ScreenType, GameState> screens)
        {
            _screens = screens;
        }

        public GameState GetScreen(ScreenType screenType)
        {
            return _screens[screenType];
        }
    }
}