using System;

namespace Domain.WorldElements
{
    public interface IWorldElement
    {
        String Name { get; }
        Coordinates Coordinates { get; set; }
 
    }
}