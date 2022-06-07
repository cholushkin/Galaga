using System;
using System.Collections.Generic;

namespace Galaga.Game
{
    [Serializable]
    public class ConfigLevel
    {
        [Serializable]
        public class Monster
        {
            public string Name;
            public float Speed;
            public float Health;
            public int ScoreAward;
            public int Weapon;
        }
        public List<string> MonsterGrid;
        public List<Monster> MonsterConfig;
    }

    [Serializable]
    public class ConfigShip
    {
        public int Lifes;
        public float Speed;
        public float ShootDelay;
        public float Damage;
    }
}

