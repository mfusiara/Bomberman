using Domain.WorldElements;

namespace Domain.Enemies
{
    public interface IEnemy : IMortal, IWorldElement
    {
        void Attack(IMortal element);
        void Move();
    }
}