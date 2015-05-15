using System;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.GameScreens
{
    public class HelpScreen : MenuScreen
    {
        private LinkLabel _back;
        private Label _content;
        private Label _gameControls;
        private Label _rating;
        public HelpScreen(Game1 game, IGameStateManager gameStateManager) : base(game, gameStateManager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = _game.Content;

            _back = CreateMenuItem("Powrot");

            var font = content.Load<SpriteFont>(@"Fonts\ControlFont15");
            _content = new Label
            {
                Position = new Vector2(50,150),
                SpriteFont = font,
                Text = "Gracz rozpoczyna rozgrywkę z 3 punktami życia oraz z 10 bombami. Głównym celem \n" + 
                "gracza jest zdobycie klucza, a następnie dotarcie do drzwi, które prowadzą do \n" + 
                "następnego poziomu. Ponadto w dotarciu do celu przeciwnikowi przeszkadzają \n" + 
                "przeciwnicy. Za zabicie przeciwników, zdobycie klucza oraz przejście do \n" + 
                "następnego poziomu gracz otrzymuje punkty (rozgrywkę rozpoczyna z 0 pkt). \n" + 
                "Przy przejściu do następnej tury zostaje zachowana liczba punktów życia i bomb. \n" + 
                "Gra składa się z pięciu poziomów, zatem po przejściu piątej planszy gra zostaje \n" + 
                "zakończona sukcesem.",
            };
            _gameControls = new Label
            {
                Position = new Vector2(100, 410),
                SpriteFont = font,
                Text = "Sterowanie:\n\n" +
                       "Góra  - Up/W\n" +
                       "Dół   - Down/S\n" +
                       "Lewo  - Left/A\n" +
                       "Prawo - Right/D\n" +
                       "Postawienie bomby - SPACE\n" +
                       "Pauza             - ESC"
            };

            _rating = new Label
            {
                Position = new Vector2(550, 410),
                SpriteFont = font,
                Text = String.Concat(
                "Punktacja:\n\n", 
                "Kolejny poziom      - ", _game.Scoring.NextLevel, "\n",
                "Odnalezienie klucza - ", _game.Scoring.KeyFounded, "\n",
                "Zabicie przeciwnika \n",
                "  Wombata  -  ", _game.Scoring.WombatKilled, "\n",
                "  Dziobaka -  ", _game.Scoring.PlatypusKilled, "\n",
                "  Tajpana  -  ", _game.Scoring.TaipanKilled, "\n",
                "  Kangura  -  ", _game.Scoring.KangarooKilled, "\n",
                "  Koali    - ", _game.Scoring.KoalaKilled),
            };

            ControlManager.Add(_back);
            ControlManager.Add(_content);
            ControlManager.Add(_gameControls);
            ControlManager.Add(_rating);

            ControlManager.NextControl();

            ControlManager.FocusChanged += ControlManager_SetArrowPositionAlligned;

            Vector2 position = new Vector2(420, 280);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel)
                {
                    if (c.Size.X > MaxItemWidth)
                        MaxItemWidth = c.Size.X;

                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
            }

            _back.Position = new Vector2(_game.ScreenRectangle.Left + 100, _game.ScreenRectangle.Bottom - 100);

            ControlManager_SetArrowPositionAlligned(_back, null);
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _back)
                GameStateManager.PopState();
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

        private LinkLabel CreateMenuItem(String text)
        {
            var result = new LinkLabel(text);
            result.Size = result.SpriteFont.MeasureString(result.Text);
            result.Selected += MenuItem_Selected;

            return result;
        }
    }
}