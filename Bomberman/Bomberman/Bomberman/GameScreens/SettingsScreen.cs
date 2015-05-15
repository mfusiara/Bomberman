using System;
using DataStorage;
using DataStorage.Enumerations;
using DataStorage.Services;
using GameLibrary;
using GameLibrary.Controls;
using GameLibrary.Controls.Selectors;
using GameLibrary.Input;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class SettingsScreen : MenuScreen
    {
        private readonly ISettingsUpdater _settingsUpdater;
        private readonly IKeyActionsManager _keyActionsManager;
        private LinkLabel _back;
        private CheckBox _music;
        private CheckBox _sfx;
        private CheckBox _controlArrows;
        private CheckBox _controlWsad;
        private Label _controlLabel;
        private LinkLabel _confirm;

        public SettingsScreen(Game1 game, 
            IGameStateManager gameStateManager, 
            ISettingsUpdater settingsUpdater, 
            IKeyActionsManager keyActionsManager) 
            : base(game, gameStateManager)
        {
            _settingsUpdater = settingsUpdater;
            _keyActionsManager = keyActionsManager;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            AddControls();
            SetControlsValues(_game.UserSettings);
        }

        private void SetControlsValues(UserSettings userSettings)
        {
            _music.IsChecked = userSettings.Music;
            _sfx.IsChecked = userSettings.SFX;
            _controlArrows.IsChecked = userSettings.Control == ControlType.ARROWS;
            _controlWsad.IsChecked = userSettings.Control == ControlType.WSAD;
        }

        private void AddControls()
        {
            _music = new CheckBox(_game.GraphicsDevice) { Text = "Muzyka", LabelPosition = Positions.Left };
            _music.Selected += MenuItem_Selected;
            _sfx = new CheckBox(_game.GraphicsDevice) { Text = "SFX", LabelPosition = Positions.Left };
            _sfx.Selected += MenuItem_Selected;
            _controlArrows = new CheckBox(_game.GraphicsDevice) { Text = "Strzałki", LabelPosition = Positions.Right };
            _controlArrows.Selected += MenuItem_Selected;
            _controlWsad = new CheckBox(_game.GraphicsDevice) { Text = "WSAD", LabelPosition = Positions.Right };
            _controlWsad.Selected += MenuItem_Selected;
            _controlLabel = new Label
            {
                Text = "Sterowanie",
            };

            var controlGroupManager = new SelectionGroupManager();
            controlGroupManager.Add(_controlArrows);
            controlGroupManager.Add(_controlWsad);

            _back = CreateMenuItem("Powrót", MenuItem_Selected);
            _confirm = CreateMenuItem("Zatwierdź", MenuItem_Selected);

            _back.Position = new Vector2(_game.ScreenRectangle.Left + 100, _game.ScreenRectangle.Bottom - 100);
            _confirm.Position = new Vector2(_game.ScreenRectangle.Right - 200, _back.Position.Y);
            _controlLabel.Position = new Vector2(420, 280);
            _controlArrows.Position = new Vector2(360, _controlLabel.Position.Y + 50);
            _controlWsad.Position = new Vector2(_controlArrows.Position.X + _controlArrows.GetWidth() + 40, _controlArrows.Position.Y);
            _music.Position = new Vector2(410, _controlArrows.Position.Y + 80);
            _sfx.Position = new Vector2(459, _music.Position.Y + 50);

            InitializeControlManager();
        }

        private void InitializeControlManager()
        {
            ControlManager.Add(_back);
            ControlManager.Add(_controlArrows);
            ControlManager.Add(_controlWsad);
            ControlManager.Add(_music);
            ControlManager.Add(_sfx);
            ControlManager.Add(_controlLabel);
            ControlManager.Add(_confirm);

            ControlManager.NextControl();
            ControlManager.FocusChanged += ControlManager_SetArrowPosition;
            ControlManager_SetArrowPosition(_back, null);
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _back)
                GameStateManager.PopState();
            var checkbox = sender as CheckBox;
            if (checkbox != null) checkbox.ChangeState();
            if (sender == _confirm)
            {
                _game.UserSettings.Music = _music.IsChecked;
                _game.UserSettings.SFX = _sfx.IsChecked;
                _game.UserSettings.Control = _controlArrows.IsChecked ? ControlType.ARROWS : ControlType.WSAD;
                _settingsUpdater.SaveSettings(_game.UserSettings);
                _keyActionsManager.ControlType = _game.UserSettings.Control;
                GameStateManager.PopState();
                if(_music.IsChecked) _game.MusicManager.Play();
                else _game.MusicManager.Stop();
                SoundEffectPlayer.Instance.Enabled = _game.UserSettings.SFX;
            }

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