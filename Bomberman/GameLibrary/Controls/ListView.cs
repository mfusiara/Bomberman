using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Controls
{
    public class ListView<T> : Control
    {
        private readonly IList<T> _items;
        private readonly IList<Label> _labels;
        private readonly int _labelHeight = 30;
        private Color _fontColor;

        public new Vector2 Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                UpdatePositions();
            }
        }

        public Color FontColor
        {
            get { return _fontColor; }
            set
            {
                _fontColor = value;
                foreach (var label in _labels)
                    label.Color = _fontColor;
            }
        }

        public ListView(IEnumerable<T> items)
        {
            if(items == null) items = new List<T>();
            _items = items as IList<T> ?? items.ToList();
            _labels = new List<Label>(_items.Count);
            for (int i = 0; i < _items.Count; i++) _labels.Add(new Label{Size = new Vector2(10)});
        }

        public void DisplayFormat(Func<T, String> format)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _labels[i].Text = format(_items[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var label in _labels) 
                label.Draw(spriteBatch);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {

        }

        private void UpdatePositions()
        {
            var position = Position;
            foreach (var label in _labels)
            {
                label.Position = position;
                position.Y += _labelHeight;
            }
        }
    }
}