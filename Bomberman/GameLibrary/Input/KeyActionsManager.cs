using System;
using System.Collections.Generic;
using DataStorage.Enumerations;
using Domain.WorldElements;
using GameLibrary.WorldManagement;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    public interface IKeyActionsManager
    {
        Dictionary<Keys, Action<ExtendedWorld>> KeyActionsMapping { get; }
        Dictionary<Keys, Action<ExtendedWorld>> SinglePressedKeyActionsMapping { get; }
        ControlType ControlType { get; set; }
        event Action ControlTypeChanged;
    }

    public class KeyActionsManager : IKeyActionsManager
    {
        private ControlType _controlType;
        public Dictionary<Keys, Action<ExtendedWorld>> SinglePressedKeyActionsMapping { get; private set; }
        public Dictionary<Keys, Action<ExtendedWorld>> KeyActionsMapping { get; private set; }

        public ControlType ControlType
        {
            get { return _controlType; }
            set
            {
                _controlType = value;
                UpdateKeyActionMapping(_controlType);
                OnControlTypeChanged();
            }
        }

        public event Action ControlTypeChanged;

        public KeyActionsManager()
        {
            UpdateKeyActionMapping(ControlType.ARROWS);
            UpdateSinglePressKeyActions();
        }

        private void UpdateKeyActionMapping(ControlType controlType)
        {
            var up = Keys.Up;
            var down = Keys.Down;
            var left = Keys.Left;
            var right = Keys.Right;
            if (controlType == ControlType.WSAD)
            {
                up = Keys.W;
                down = Keys.S;
                left = Keys.A;
                right = Keys.D;
            }

            KeyActionsMapping = new Dictionary<Keys, Action<ExtendedWorld>>
            {
                {up, exWorld => exWorld.MovePlayer(new Coordinates(0, -1))},
                {down, exWorld => exWorld.MovePlayer(new Coordinates(0, 1))},
                {left, exWorld => exWorld.MovePlayer(new Coordinates(-1, 0))},
                {right, exWorld => exWorld.MovePlayer(new Coordinates(1, 0))},
            };
        }

        private void UpdateSinglePressKeyActions()
        {
            SinglePressedKeyActionsMapping = new Dictionary<Keys, Action<ExtendedWorld>>
            {
                {Keys.Space, exWorld => exWorld.PlaceBomb(exWorld.World.Player.Coordinates)},
                {Keys.Escape, exWorld => exWorld.Pause()},
            };
        }

        protected virtual void OnControlTypeChanged()
        {
            var handler = ControlTypeChanged;
            if (handler != null) handler();
        }
    }
}