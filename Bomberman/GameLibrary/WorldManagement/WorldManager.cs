using System;
using System.Collections.Generic;
using DataStorage;
using Domain;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.WorldManagement
{
    public class WorldManager
    {
        private readonly GameInfo _gameInfo;
        private readonly IKeyActionsManager _keyActionsManager;
        private IDictionary<Keys, Action<ExtendedWorld>> _keyActionsMapping;
        private readonly IDictionary<Keys, Action<ExtendedWorld>> _singlePressedKeyActionsMapping;
        private readonly ExtendedWorld _exWorld;
        public event Action Paused;
        public WorldManager(World world, WorldAssembler worldAssembler, GameInfo gameInfo, Scoring scoring, IKeyActionsManager keyActionsManager)
        {
            _gameInfo = gameInfo;
            _keyActionsManager = keyActionsManager;
            _keyActionsMapping = _keyActionsManager.KeyActionsMapping;
            _singlePressedKeyActionsMapping = _keyActionsManager.SinglePressedKeyActionsMapping;
            _exWorld = worldAssembler.Build(world);
            _exWorld.Stats = _gameInfo.Stats;
            _exWorld.Scoring = scoring;
            _exWorld.Paused += OnPaused;

            _keyActionsManager.ControlTypeChanged += () => _keyActionsMapping = _keyActionsManager.KeyActionsMapping;
        }


        public void Update(GameTime gameTime, Keys[] pressedKeys, IEnumerable<Keys> singlePressedKeys)
        {
            foreach (var pressedKey in pressedKeys)
            {
                if (_keyActionsMapping.ContainsKey(pressedKey))
                {
                    _keyActionsMapping[pressedKey](_exWorld);
                }
            }
            foreach (var pressedKey in singlePressedKeys)
            {
                if (_singlePressedKeyActionsMapping.ContainsKey(pressedKey))
                {
                    _singlePressedKeyActionsMapping[pressedKey](_exWorld);
                }
            }

            _exWorld.MoveEnemies();
            _exWorld.UpdateSprites(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gamTime)
        {
            _exWorld.ForEachAnimiation(animation =>
            {
                if (animation != null)
                    animation.Draw(gamTime, spriteBatch);
            });

            foreach (var graphics in _exWorld.Graphics)
            {
                graphics.Draw(spriteBatch);
            }
            foreach (var control in _exWorld.Controls)
            {
                control.Draw(spriteBatch);
            }
        }

        protected virtual void OnPaused()
        {
            var handler = Paused;
            if (handler != null) handler();
        }

    }
}