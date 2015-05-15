using System;
using System.Linq;
using Domain.DTO;
using Domain.Enemies;
using Domain.Players;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;

namespace Domain.Assemblers
{
    public interface IDTOAssembler
    {
        WorldDTO GetDTO(World world);
        PlayerDTO GetDTO(PlayerBase player);
        EnemyDTO GetDTO(Enemy enemy);
        WallDTO GetDTO(Wall wall);
        BombDTO GetDTO(Bomb bomb);
        AidKitDTO GetDTO(AidKit bomb);
    }

    public class DTOAssembler : IDTOAssembler
    {

        public WorldDTO GetDTO(World world)
        {
            var worldDto = new WorldDTO
            {
                Width = world.Width/world.FieldSize,
                Height = world.Height/world.FieldSize,
                Player = GetDTO(world.Player),
                Enemies = world.Enemies.Select(GetDTO).ToArray(),
                Walls = world.Walls.Select(GetDTO).ToArray(),
                Key = world.Key != null ? GetDTO(world.Key) : null,
                Door = GetDTO(world.Door),
                Bombs = world.Bombs.Select(GetDTO).ToArray(),
                BombSets = world.BombSets.Select(GetDTO).ToArray(),
            };
            FixCoordinates(worldDto, world.FieldSize, world.Yoffset);

            return worldDto;
        }

        private DoorDTO GetDTO(Door door)
        {
            return new DoorDTO
            {
                Coordinates = (Coordinates) door.Coordinates.Clone(),
                Key = door.Key,
                Name = door.Name,
            };
        }

        public PlayerDTO GetDTO(PlayerBase player)
        {
            return new PlayerDTO
            {
                Name = player.Name,
                Coordinates = (Coordinates) player.Coordinates.Clone(),
                Hitpoints = player.Hitpoints,
                Key = player.Key != null ? GetDTO(player.Key) : null,
                Bombs = player.Bombs.Select(GetDTO).ToArray(),
            };
        }

        public EnemyDTO GetDTO(Enemy enemy)
        {
            EnemyDTO result = null;
            if(enemy is Kangaroo)
                result = new KangarooDTO();
            if(enemy is Koala)
                result = new KoalaDTO();
            if(enemy is Platypus)
                result = new PlatypusDTO();
            if(enemy is Taipan)
                result = new TaipanDTO();
            if(enemy is Wombat)
                result = new WombatDTO();

            if(result != null)
                result.Coordinates = (Coordinates) enemy.Coordinates.Clone();
            return result;
        }

        public WallDTO GetDTO(Wall wall)
        {
            var wallDto = new WallDTO
            {
                Coordinates = (Coordinates) wall.Coordinates.Clone(),
            };
            if (wall is DestructibleWall)
            {
                wallDto.IsDestructible = true;
                wallDto.Hitpoints = ((DestructibleWall) wall).Hitpoints;
            }

            return wallDto;
        }

        private KeyDTO GetDTO(Key key)
        {
            return new KeyDTO
            {
                Coordinates = key.Coordinates != null ? (Coordinates) key.Coordinates.Clone() : null,
                Key = key.Value,
                Name = key.Name,
            };
        }

        public BombDTO GetDTO(Bomb bomb)
        {
            return new BombDTO
            {
                Coordinates = (Coordinates) bomb.Coordinates.Clone(),
                Name = bomb.Name,
                DestructionField = bomb.DestructionField,
                TimeSpan = bomb.TimeSpan,
            };
        }

        public AidKitDTO GetDTO(AidKit aidKit)
        {
            return new AidKitDTO
            {
                Coordinates = aidKit.Coordinates,
                Hitpoints = aidKit.Hitpoints,
            };
        }

        public BombSetDTO GetDTO(BombSet bombSet)
        {
            return new BombSetDTO
            {
                Bombs = bombSet.Bombs.Select(GetDTO).ToList(),
                Coordinates = bombSet.Coordinates
            };
        }

        private void FixCoordinates(WorldDTO worldDto, int fieldSize, int yoffset)
        {
            Func<Coordinates, Coordinates> multiply = c => new Coordinates(c.X/fieldSize, (c.Y - yoffset)/fieldSize);
            foreach (var enemyDto in worldDto.Enemies)
                enemyDto.Coordinates = multiply(enemyDto.Coordinates);

            foreach (var wallDto in worldDto.Walls)
                wallDto.Coordinates = multiply(wallDto.Coordinates);

            foreach (var bombDto in worldDto.Bombs)
                bombDto.Coordinates = multiply(bombDto.Coordinates);

            foreach (var bombSetDto in worldDto.BombSets)
                bombSetDto.Coordinates = multiply(bombSetDto.Coordinates);

            worldDto.Player.Coordinates = multiply(worldDto.Player.Coordinates);
            worldDto.Door.Coordinates = multiply(worldDto.Door.Coordinates);
            if(worldDto.Key != null) worldDto.Key.Coordinates = multiply(worldDto.Key.Coordinates);
        }
    }
}