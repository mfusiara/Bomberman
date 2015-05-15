
using System.Collections.Generic;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.GameScreens
{
    public abstract class GameState : DrawableGameComponent
    {
        protected readonly Game1 _game;
        protected readonly IGameStateManager GameStateManager;
        protected ControlManager ControlManager;
        IList<GameComponent> childComponents;

        protected GameState(Game1 game, IGameStateManager gameStateManager) : base(game)
        {
            _game = game;
            GameStateManager = gameStateManager;
            childComponents = new List<GameComponent>();
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            SpriteFont menuFont = content.Load<SpriteFont>(@"Fonts\ControlFont");
            ControlManager = new ControlManager(menuFont);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents)
            {
                var gameComponent = component as DrawableGameComponent;
                if (gameComponent != null)
                {
                    var drawComponent = gameComponent;

                    if (drawComponent.Visible)
                        drawComponent.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        public void StateChange()
        {
            if (GameStateManager.CurrentState == this)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (GameComponent component in childComponents)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }
    }
}