using System;
using DataStorage.Services;
using GameLibrary;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class LoginScreen : MenuScreen
    {
        private readonly ILoginService _loginService;
        private LinkLabel _startLabel;
        private TextBox _loginTextBox;
        private Label _loginLabel;

        public LoginScreen(Game1 game, 
            IGameStateManager gameStateManager, 
            ILoginService loginService) : base(game, gameStateManager)
        {
            _loginService = loginService;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _loginLabel = new Label()
            {
                Text = "Login:",
                Color = Color.White,
                TabStop = false,
                Position = new Vector2(420, 480),
            };

            _loginTextBox = new TextBox
            {
                MaxLength = 10,
                HasFocus = true,
                Position = new Vector2(520, 480)
            };
            _loginTextBox.Size = _loginTextBox.SpriteFont.MeasureString("DUMMY TEXT");
            _loginTextBox.Selected += LoginTextBoxOnSelected;

            _startLabel = new LinkLabel
            {
                Position = new Vector2(350, 600),
                Text = "Wciśnij ENTER aby zacząć",
                Color = Color.White,
                TabStop = true,
            };

            ControlManager.Add(_loginLabel);
            ControlManager.Add(_loginTextBox);
            ControlManager.Add(_startLabel);
        }

        private void LoginTextBoxOnSelected(object sender, EventArgs eventArgs)
        {
            if (String.IsNullOrEmpty(_loginTextBox.Text)) return;

            _game.User = _loginService.Login(_loginTextBox.Text);
            _game.UserSettings = _loginService.GetUserSettings(_game.User.Id);

            GameStateManager.ChangeState(ScreenType.MainMenu);
            if (_game.UserSettings.Music) _game.MusicManager.Play();
            SoundEffectPlayer.Instance.Enabled = _game.UserSettings.SFX;
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Begin();

            base.Draw(gameTime);
            ControlManager.Draw(_game.SpriteBatch);

            _game.SpriteBatch.End();
        }
    }
}