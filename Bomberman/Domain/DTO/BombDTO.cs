using System;

namespace Domain.DTO
{
    public class BombDTO : WorldElementDTO
    {
        public ushort DestructionField { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }
}