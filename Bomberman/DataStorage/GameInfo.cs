using System;
using Domain;
using Domain.DTO;

namespace DataStorage
{
    public class GameInfo
    {
        public int UserId { get; set; }
        public WorldDTO World { get; set; }
        public GameStats Stats { get; set; }
        public String Name { get; set; }
    }
}