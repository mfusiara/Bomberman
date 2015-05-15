using System;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Controls.Selectors
{
    public class CheckBox : Control
    {
        private int _rectangleMargin = 10;
        private readonly Label _label;
        private Vector2 _position;
        private CheckRectangle _checkRectangle;
        private Positions _labelPosition = Positions.Left;
        private Color _selectedColor = Color.Red;
        private bool _isChecked;

        public event EventHandler SelectionChange;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                _checkRectangle.IsChecked = _isChecked;
                OnSelectionChange();
            }
        }

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        public Positions LabelPosition
        {
            get { return _labelPosition; }
            set
            {
                _labelPosition = value;
                UpdatePosition();
            }
        }

        public new String Text
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
                UpdatePosition();
            }
        }

        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdatePosition();
            }
        }

        public CheckBox(GraphicsDevice graphicsDevice)
        {
            TabStop = true;
            Size = new Vector2(15);
            _label = new Label
            {
                Color = Color,
            };
            _checkRectangle = new CheckRectangle(graphicsDevice);
        }

        public override float GetWidth()
        {
            return _checkRectangle.Width + _rectangleMargin + _label.GetWidth();
        }

        public void ChangeState()
        {
            IsChecked = !IsChecked;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _checkRectangle.Draw(spriteBatch);
            _label.Color = HasFocus ? SelectedColor : Color;
            _label.Draw(spriteBatch);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (!HasFocus)
                return;

            if (InputHandler.KeyReleased(Keys.Enter) ||
                InputHandler.ButtonReleased(Buttons.A, playerIndex))
                base.OnSelected(null);
        }

        private void UpdatePosition()
        {
            if (LabelPosition == Positions.Left)
            {
                _label.Position = Position;
                _checkRectangle.Position = new Vector2(Position.X + _label.GetWidth() + _rectangleMargin, Position.Y);
            }
            else
            {
                _checkRectangle.Position = Position;
                _label.Position = new Vector2(Position.X + _checkRectangle.Width + _rectangleMargin, Position.Y);
            }

            Size = new Vector2(_label.GetWidth() + _rectangleMargin + _checkRectangle.Width, _checkRectangle.Width);
        }

        protected virtual void OnSelectionChange()
        {
            var handler = SelectionChange;
            if (handler != null) handler(this, EventArgs.Empty);    
        }
    }
}