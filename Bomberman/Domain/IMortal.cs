using System;
using Domain.Enemies;
using Domain.WorldElements;

namespace Domain
{
    public interface IMortal
    {
        double Hitpoints { get; }
        double ReceiveAttack(double points = 1);
        event Action<WorldElement> Dead;
    }
}