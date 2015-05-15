using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Domain;
using Domain.Assemblers;
using Domain.Enemies;
using Domain.Enemies.Attacks;
using Domain.Enemies.Motion;
using Domain.Players;
using Domain.Serialization;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;
using Microsoft.Win32;
using Key = System.Windows.Input.Key;

namespace WorldEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int _rowNumber = 12;
        private const int _columnNumber = 16;
        private readonly WorldElement[,] _worldElements;
        private Func<Coordinates, WorldElement> _createWorldElement;
        private Brush _background;

        public MainWindow()
        {
            InitializeComponent();

            _worldElements = new WorldElement[_columnNumber, _rowNumber];
            for (int i = 0; i < _rowNumber; i++)
                Grid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < _columnNumber; i++)
                Grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int x = 0; x < _rowNumber; x++)
            {
                for (int y = 0; y < _columnNumber; y++)
                {
                    var button = new Button(){Content = new Coordinates(y, x)};
                    button.Click += ButtonOnClick;
                    Grid.SetRow(button, x);
                    Grid.SetColumn(button, y);
                    Grid.Children.Add(button);
                }
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var button = sender as Button;
            var c = button.Content as Coordinates;

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _worldElements[c.X, c.Y] = null;
                button.Background = new SolidColorBrush(Colors.White);
                return;
            }
            _worldElements[c.X, c.Y] = _createWorldElement(c);
            button.Background = _background;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var elements = new List<WorldElement>();
            foreach (var worldElement in _worldElements)
            {
                if(worldElement != null)
                    elements.Add(worldElement);
            }
            var bombs = new List<Bomb>();
            for (int i = 0; i < 10; i++) bombs.Add(new Bomb());
            var world = new World(_columnNumber, _rowNumber, 
                new Player(new PlayerMotion(), new Coordinates(0, 0), bombs, 3, new Domain.WorldElements.Key(2)), elements.OfType<Enemy>().ToList(),
                elements.OfType<Wall>().ToList(), 
                new Collection<BombSet>(),
                new Collection<AidKit>(), 
                new Domain.WorldElements.Key(2, new Coordinates(15, 0)), 
                new Door(2, new Coordinates(15, 11)));

            var worldAssembler = new DTOAssembler();
            var serializer = new WorldSerializer();
            

            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = "(*.xml)|*.xml";
            if (fileDialog.ShowDialog() == true)
            {
                serializer.Serialize(worldAssembler.GetDTO(world), fileDialog.FileName);
            }
        }

        private void KoalaChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new Koala(c, new ZeroHitpointAttack(), new NoMotion(), 1, 1);
            _background = (sender as Control).Background;
        }
        private void PlatypusChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new Platypus(c, new ZeroHitpointAttack(), new NoMotion(), 1, 1);
            _background = (sender as Control).Background;
        }
        private void WombatChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new Wombat(c, new ZeroHitpointAttack(), new NoMotion(), 1, 1);
            _background = (sender as Control).Background;
        }
        private void KangarooChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new Kangaroo(c, new ZeroHitpointAttack(), new NoMotion(), 1, 1);
            _background = (sender as Control).Background;
        }
        private void TaipanChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new Taipan(c, new ZeroHitpointAttack(), new NoMotion(), 1, 1);
            _background = (sender as Control).Background;
        }

        private void WallChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new Wall(c);
            _background = (sender as Control).Background;
        }
        private void DestructibleWallChecked(object sender, RoutedEventArgs e)
        {
            _createWorldElement = c => new DestructibleWall(c);
            _background = (sender as Control).Background;
        }
    }

}
