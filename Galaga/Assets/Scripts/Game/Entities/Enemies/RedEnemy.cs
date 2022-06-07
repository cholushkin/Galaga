using UnityEngine;

namespace Galaga.Game
{
    public class RedEnemy : BlueEnemy
    {
        private float _nextRocket;

        private const float ReloadTime = 0.5f;
        private int _rocketCount;

        protected override void OnStateChange(State newState)
        {
            if (newState == State.FlyToHero) 
            {
                // full reload
                _rocketCount = Weapon;
                _nextRocket = ReloadTime; 
            }
        }

        protected override void Update()
        {
            base.Update();

            // process states
            if (_state == State.FlyToHero)
            {
                _nextRocket -= Time.deltaTime;
                if (_nextRocket < 0f && _rocketCount > 0 )
                {
                    --_rocketCount;
                    _nextRocket = ReloadTime; // reloading
                    var rocket = Factory.Create("RocketBlue", _gameProcessor.Projectiles, transform.position);
                    rocket.GetComponent<RocketBlue>().HeroController = _gameProcessor.HeroController; // set enemy accessor for rocket
                }
            }
        }

        public override void StartThink()
        {
            if (_state != State.StayOnGrid)
                return;

            SetState(State.FlyToHero);
            StartFlyToHero();
        }
    }
}
