using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public float Delay;
        public Grid Grid;
        public int Raw;
        public bool IsReverseOrder;
        public bool IsEmpty { get; private set; }
        public EnemySpawner NextSpawner;

        private GameProcessor _gameProcessor;

        private const char CharEmptySlot = '.';
        private const char CharRedMonster = 'r';
        private const char CharGreenMonster = 'g';
        private const char CharBlueMonster = 'b';

        void Awake()
        {
            Reload();
            _gameProcessor = GetComponentInParent<GameProcessor>();
        }

        public void Reload()
        {
            IsEmpty = false;
        }

        public void Spawn()
        {
            if (IsEmpty)
                return;
            StartCoroutine(Spawning());
        }

        public int GetMobCount()
        {
            var config = Grid.GetRawConfig(Raw);
            return config.Length - config.Count(x => x == CharEmptySlot);
        }

        IEnumerator Spawning()
        {
            var rawConfig = Grid.GetRawConfig(Raw);

            for (int i = 0; i < rawConfig.Length; ++i)
            {
                var c = rawConfig[i];
                if (c == CharEmptySlot) // skip slot
                    continue;
                yield return new WaitForSeconds(Delay);

                var monsterName = MapSymbolToMonsterName(c);

                var gObj = Factory.Create(monsterName, _gameProcessor.Monsters, transform.position);
                var monsterConfig = _gameProcessor.GetLevelConfiguration()
                    .MonsterConfig.FirstOrDefault(x => x.Name == monsterName);
                gObj.GetComponent<BaseEnemy>().SetContext(_gameProcessor, monsterConfig);

                var gridFollower = gObj.GetComponent<FollowerGrid>();
                gridFollower.GridX = IsReverseOrder ? rawConfig.Length - i - 1 : i;
                gridFollower.GridY = Raw;
                gridFollower.Target = _gameProcessor.Grid;
            }

            yield return new WaitForSeconds(Delay);
            IsEmpty = true;

            if (NextSpawner != null)
                NextSpawner.Spawn();
        }

        private string MapSymbolToMonsterName(char c)
        {
            Assert.IsTrue(c == 'r' || c == 'g' || c == 'b');
            if (c == CharRedMonster)
                return "Red";
            if (c == CharGreenMonster)
                return "Green";
            if (c == CharBlueMonster)
                return "Blue";
            return "";
        }
    }
}
