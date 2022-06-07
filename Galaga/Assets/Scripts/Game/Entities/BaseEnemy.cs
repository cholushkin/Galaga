using UnityEngine;

namespace Galaga.Game
{
    public class BaseEnemy : MonoBehaviour
    {
        public enum State
        {
            StayOnGrid,
            FlyToHero,
            Waiting,
            FlyToBottom,
        }

        public float Health;
        public float Speed;
        public int ScoreAward;
        public int Weapon;
        protected GameProcessor _gameProcessor;
        protected Follower _follower;
        protected State _state;


        protected virtual void Awake()
        {
            _follower = GetComponent<Follower>();
        }

        public void SetContext(GameProcessor gameProcessor, ConfigLevel.Monster config)
        {
            _gameProcessor = gameProcessor;

            // apply config
            gameObject.name = config.Name;
            Speed = config.Speed;
            ScoreAward = config.ScoreAward;
            Health = config.Health;
            Weapon = config.Weapon;

            // speed parameter is percent [0..1] of available speed in range [slowest-fastest]
            const float slowestValue = 3f;
            const float fastestValue = 1f;
            var speedFactor = Mathf.Clamp(config.Speed, 0f, 1f);
            speedFactor = slowestValue - (slowestValue - fastestValue) * speedFactor;
            _follower.smoothTime = speedFactor;
        }

        public void ReceiveDamage(float damage)
        {
            const float ParticlesDuration = 2f;
            Health -= damage;

            Factory.Create("PfxBoom", _gameProcessor.Effects, transform.position, ParticlesDuration);

            GetComponent<SpriteRenderer>().color = Color.red;

            if (Health <= 0.0f)
                Explode();
        }

        void Explode()
        {
            const float ExplosionDelay = 0.5f;
            OnBeforeDie();
            _gameProcessor.OnMonsterDied(ScoreAward);
            Destroy(gameObject);
            Factory.Create("ExplosionSmall", _gameProcessor.Effects, transform.position, ExplosionDelay);
        }

        public virtual void OnBeforeDie()
        {
        }

        public virtual void StartThink()
        {
            throw new global::System.NotImplementedException();
        }

        #region AI API
        protected void SetState(State state)
        {
            OnStateChange(state);
            _state = state;
        }

        protected virtual void OnStateChange(State newState)
        {

        }

        protected void FollowGrid(bool flag)
        {
            GetComponent<FollowerGrid>().enabled = flag;
        }

        protected void StartFlyToHeroLastPos()
        {
            FollowGrid(false);
            _follower.SetTarget(_gameProcessor.HeroController.GetHeroApproxPosition());
            _follower.enabled = true;
        }

        protected void StartFlyToHero()
        {
            FollowGrid(false);
            _follower.SetTarget(_gameProcessor.HeroController.HeroFollowPoint);
            _follower.enabled = true;
        }

        protected void StartFlyToHeroFront()
        {
            const float HeroFrontLine = 3.5f;
            FollowGrid(false);
            _follower.SetTarget(_gameProcessor.HeroController.GetHeroApproxPosition() + Vector3.up * HeroFrontLine);
            _follower.enabled = true;
        }

        protected void StartFlyToGrid()
        {
            FollowGrid(true);
            _follower.enabled = false;
        }

        protected void StartFlyToBottom()
        {
            const float BottomLine = 5f;
            _follower.SetTarget(_gameProcessor.HeroController.GetHeroApproxPosition() + Vector3.down * BottomLine);
        }

        protected void LookToHero()
        {
            Vector3 vectorToTarget = _gameProcessor.HeroController.GetHeroApproxPosition() - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.Euler(0, 0, 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime);
        }

        protected void LookToBottom()
        {
            transform.rotation = Quaternion.identity;
        }

        protected float HeroDistanceApprox()
        {
            return (_gameProcessor.HeroController.GetHeroApproxPosition() - transform.position).magnitude;
        }

        protected bool ProcessCollisionWithHero()
        {
            const float CollisionFactor = 0.25f;
            var ship = _gameProcessor.HeroController.Ship;

            if (ship == null)
                return false;
            if (_gameProcessor.HeroController.HeroFollowPoint.position.y > transform.position.y)
                return false;

            var distance = (ship.transform.position - transform.position).sqrMagnitude;
            return (distance < CollisionFactor);
        }

        protected void RespawnOnTop()
        {
            const float RespawningLine = 15f;
            SetState(State.StayOnGrid);
            LookToBottom();
            transform.position = new Vector3(transform.position.x, RespawningLine, transform.position.z);
            StartFlyToGrid();
        }
        #endregion
    }
}
