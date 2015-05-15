using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using Domain;
using Domain.Assemblers;
using Domain.DTO;
using Domain.Enemies;
using Domain.Enemies.Motion;
using Domain.Players;
using Domain.Serialization;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    public class WorldSerializerTest
    {
        private World _world;
        private WorldSerializer _worldSerializer;

        [TestFixtureSetUp]
        public void Init()
        {
            _worldSerializer = new WorldSerializer();

            var player = new Player(new PlayerMotion());
            player.CollectBombs(new List<Bomb> { new Bomb() });
            var enemies = new List<Enemy>();
            enemies.Add(new Kangaroo(new RandomMotion()));
            var walls = new List<Wall>();
            walls.Add(new Wall());
            var key = new Key(1);
            var door = new Door(1);
            var bombSets = new Collection<BombSet>();
            var aidKits = new Collection<AidKit>();

            _world = new World(10, 10, player, enemies, walls, bombSets, aidKits, key, door);
        }


        [Test]
        public void Serialize()
        {
            var path = "World.xml";
            var dtoAssembler = new DTOAssembler();
            var worldDto = dtoAssembler.GetDTO(_world);

            _worldSerializer.Serialize(worldDto, path);

            var file = new FileInfo(path);
            Assert.IsTrue(file.Exists);
        }

        [Test]
        public void Deserialize()
        {
            const string path = "World_Test.xml";

            var result = _worldSerializer.Deserialize(path);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<WorldDTO>(result);
        }


    }
}