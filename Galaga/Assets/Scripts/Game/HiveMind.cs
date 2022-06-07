using System.Linq;
using UnityEngine;

namespace Galaga.Game
{
    public class HiveMind : MonoBehaviour
    {
        enum State
        {
            Waiting, // wait while all spawners complete spawn units
            Choosing, // choose next unit and activating it
            Dead // do nothing
        }

        public float MinNextMonster;
        public float MaxNextMonster;

        private State _state;
        private GameProcessor _gameProcessor;
        private EnemySpawner[] _spawners;
        private float _nextActivating;


        public void Awake()
        {
            _gameProcessor = GetComponentInParent<GameProcessor>();
            _spawners = _gameProcessor.Spawners.GetComponentsInChildren<EnemySpawner>();
            SetState(State.Dead);
        }

        public void StartThink()
        {
            SetState(State.Waiting);
        }


        void SetState(State state)
        {
            Debug.Log("HiveMind:SetState " + state);
            _state = state;

            if (state == State.Choosing)
                NextActivating();
        }
       
        void NextActivating()
        {
            _nextActivating = Random.Range(MinNextMonster, MaxNextMonster);
        }

        void Update()
        {
            if (_state == State.Waiting)
            {
                var isAllSpawned = _spawners.FirstOrDefault(x => !x.IsEmpty) == null;
                if (isAllSpawned)
                    SetState(State.Choosing);
            }
            else if (_state == State.Choosing)
            {
                _nextActivating -= Time.deltaTime;
                if (_nextActivating < 0f)
                {
                    var rndMonster = _gameProcessor.Monsters.GetChild(Random.Range(0, _gameProcessor.Monsters.childCount));
                    rndMonster.GetComponent<BaseEnemy>().StartThink();
                    if(!_gameProcessor.IsLevelClear)
                        NextActivating();
                }
            }
        }
    }
}
