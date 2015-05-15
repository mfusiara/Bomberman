using Domain.DTO;
using Domain.Enemies.Attacks;
using Domain.Enemies.Motion;

namespace Domain.Enemies.Factories
{
    public interface IEnemyFactory
    {
        Enemy Create(EnemyDTO enemyDto);
    }

    public class EnemyFactory : IEnemyFactory
    {
        private readonly IScoring _scoring;

        public EnemyFactory(IScoring scoring)
        {
            _scoring = scoring;
        }

        public Enemy Create(EnemyDTO enemyDto)
        {
            Enemy enemy = null;
            if (enemyDto is KangarooDTO)
                return CreateKangaroo(enemyDto);
            if (enemyDto is KoalaDTO)
                return CreateKoala(enemyDto);
            if (enemyDto is TaipanDTO)
                return CreateTaipan(enemyDto);
            if (enemyDto is WombatDTO)
                return CreateWombat(enemyDto);
            if (enemyDto is PlatypusDTO)
                return CreatePlatypus(enemyDto);
            
            return enemy;
        }

        private Wombat CreateWombat(EnemyDTO enemyDto)
        {
            return new Wombat(enemyDto.Coordinates,
                    new HalfHitpointContactAttack(),
                    new RandomMotion(), 
                    enemyDto.Hitpoints,
                    _scoring.WombatKilled);
        }

        private Taipan CreateTaipan(EnemyDTO enemyDto)
        {
            return new Taipan(enemyDto.Coordinates,
                    new OneHitpointContactAttack(),
                    new UpDownMotion(), 
                    enemyDto.Hitpoints,
                    _scoring.TaipanKilled);
        }

        private Platypus CreatePlatypus(EnemyDTO enemyDto)
        {
            return new Platypus(enemyDto.Coordinates,
                    new TwoHitpointsContactAttack(),
                    new NoMotion(),
                    enemyDto.Hitpoints,
                    _scoring.PlatypusKilled);    
        }

        private Koala CreateKoala(EnemyDTO enemyDto)
        {
            return new Koala(enemyDto.Coordinates,
                    new ZeroHitpointAttack(),
                    new NoMotion(),
                    enemyDto.Hitpoints,
                    _scoring.KoalaKilled);
        }

        private Kangaroo CreateKangaroo(EnemyDTO enemyDto)
        {
            return new Kangaroo(enemyDto.Coordinates,
                    new OneHitpointContactAttack(),
                    new RandomMotion(),
                    enemyDto.Hitpoints,
                    _scoring.KangarooKilled);
        }
    }
}