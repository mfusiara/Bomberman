using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public interface IGameStateManager : IGameComponent
    {
        event Action StateChanged;
        GameState CurrentState { get; }
        bool Enabled { get; set; }
        int UpdateOrder { get; set; }
        Game Game { get; }
        void PopState();
        void RemoveState();
        void PushState(GameState newState);
        void PushState(ScreenType screenType);
        void AddState(GameState newState);
        void ChangeState(ScreenType newState);
        void Update(GameTime gameTime);
        void Dispose();
        event EventHandler<EventArgs> EnabledChanged;
        event EventHandler<EventArgs> UpdateOrderChanged;
        event EventHandler<EventArgs> Disposed;
    }

    public class GameStateManager : GameComponent, IGameStateManager
    {
        private const int StartDrawOrder = 5000;
        private const int DrawOrderInc = 100;
        private readonly IScreenFactory _screenFactory;
        private readonly Stack<GameState> _gameStates = new Stack<GameState>();
        private int _drawOrder;

        public GameStateManager(Game game, IScreenFactory screenFactory) : base(game)
        {
            _screenFactory = screenFactory;
            _drawOrder = StartDrawOrder;
        }

        public event Action StateChanged;

        public GameState CurrentState
        {
            get { return _gameStates.Peek(); }
        }

        public void PopState()
        {
            if (_gameStates.Count > 0)
            {
                RemoveState();
                _drawOrder -= DrawOrderInc;

                OnStateChanged();
            }
        }

        public void RemoveState()
        {
            GameState state = _gameStates.Peek();

            StateChanged -= state.StateChange;
            Game.Components.Remove(state);
            _gameStates.Pop();
        }

        public void PushState(ScreenType screenType)
        {
            PushState(_screenFactory.GetScreen(screenType));
        }

        public void PushState(GameState newState)
        {
            _drawOrder += DrawOrderInc;
            newState.DrawOrder = _drawOrder;

            AddState(newState);

            OnStateChanged();
        }

        public void AddState(GameState newState)
        {
            _gameStates.Push(newState);

            Game.Components.Add(newState);

            StateChanged += newState.StateChange;
        }

        public void ChangeState(ScreenType newState)
        {
            ChangeState(_screenFactory.GetScreen(newState));
        }

        public void ChangeState(GameState newState)
        {
            while (_gameStates.Count > 0)
                RemoveState();

            newState.DrawOrder = StartDrawOrder;
            _drawOrder = StartDrawOrder;

            AddState(newState);

            OnStateChanged();
        }

        protected virtual void OnStateChanged()
        {
            Action handler = StateChanged;
            if (handler != null) handler();
        }
    }
}