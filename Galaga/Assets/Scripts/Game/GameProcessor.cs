using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.Game
{
    public class GameProcessor : MonoBehaviour
    {
        public event Action ScoreChanged = delegate { };
        public event Action LivesChanged = delegate { };

        public int Lives
        {
            get
            {
                return _lives;
            }
            set
            {
                _lives = value;
                LivesChanged();
            }
        }

        public int Scores
        {
            get
            {
                return _scores;
            }
            set
            {
                _scores = value;
                ScoreChanged();
            }
        }


        public HeroController HeroController;
        public Grid Grid;
        public EnemySpawner StartSpawner;
        public HiveMind HiveMind;
        public Transform Spawners;
        public Transform Monsters;
        public Transform Projectiles;
        public Transform Effects;

        public TextAsset ConfigShip;
        public TextAsset[] ConfigLevels;

        private ConfigShip _configShip;
        private ConfigLevel _configLevelCurrent;
        private int _levelConfigIndex;


        private int _scores;
        private int _lives;
        internal bool IsGameOver
        {
            get { return Lives < 0; }
        }
        internal bool IsLevelClear;
        private int _monstersToKill;


        void Awake()
        {
            Assert.IsNotNull(ConfigShip);
            Assert.IsTrue(ConfigLevels.Length > 0);
            _configShip = JsonUtility.FromJson<ConfigShip>(ConfigShip.text);
        }

        void Start()
        {
            Scores = 0;
            Lives = _configShip.Lifes;
            StartNextLevel();
        }

        void Update()
        {
            if (IsLevelClear)
            {
                IsLevelClear = false;
                StartNextLevel();
            }
        }

        private void StartNextLevel()
        {
            // load next level config
            _configLevelCurrent = JsonUtility.FromJson<ConfigLevel>(ConfigLevels[_levelConfigIndex].text);
            _levelConfigIndex = (_levelConfigIndex + 1) % ConfigLevels.Length;

            // reconfigure grid
            Grid.Configure(_configLevelCurrent);

            // reconfigure spawners
            var spawners = Spawners.GetComponentsInChildren<EnemySpawner>();
            _monstersToKill = spawners.Sum(x => x.GetMobCount());
            foreach (var enemySpawner in spawners)
                enemySpawner.Reload();
            StartSpawner.Spawn();
            Debug.Log("Monsters to kill: " + _monstersToKill);

            // hive mind
            HiveMind.StartThink();
        }

        public ConfigShip GetShipConfiguration()
        {
            return _configShip;
        }

        public ConfigLevel GetLevelConfiguration()
        {
            return _configLevelCurrent;
        }

        public void OnMonsterDied(int scoreAward)
        {
            Scores += scoreAward;
            --_monstersToKill;
            IsLevelClear = _monstersToKill <= 0;
            if (IsLevelClear)
                Debug.Log("Level clear");
        }

        [ContextMenu("DbgPrintConfig")]
        public void DbgPrintConfig()
        {
            Debug.Log(JsonUtility.ToJson(_configShip, true));
            Debug.Log(JsonUtility.ToJson(_configLevelCurrent, true));
        }
    }

}
