using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.Game
{
    public class HeroController : MonoBehaviour
    {
        public float Edge;
        public float Speed;
        public float ShootDelay;
        public float Damage; // how much damage can ship make

        private float CurrentShootReloadingTime;
        public Transform HeroSpawnPoint;
        public Transform HeroFollowPoint;
        public GameObject Ship { get; private set; }
        private GameProcessor _gameProcessor;


        private const float ExplosionDelay = 0.6f;
        private const float RespawnDelay = 1.5f;
        private const float ExplParticlesDuration = 2f;
        private const float RocketLifeTime = 2f;


        void Awake()
        {
            _gameProcessor = GetComponentInParent<GameProcessor>();
            Assert.IsNotNull(_gameProcessor);
        }

        void Start()
        {
            // apply config for the ship
            Speed = _gameProcessor.GetShipConfiguration().Speed;
            ShootDelay = _gameProcessor.GetShipConfiguration().ShootDelay;
            Damage = _gameProcessor.GetShipConfiguration().Damage;

            SpawnShip();
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (IsDead())
                return;

            var isMovingLeft = Input.GetKey(KeyCode.LeftArrow);
            var isMovingRight = Input.GetKey(KeyCode.RightArrow);
            var isShooting = Input.GetKey(KeyCode.Space);

            // process moving
            if (isMovingLeft || isMovingRight)
            {
                var deltaPos = (isMovingLeft ? Vector3.left : Vector3.right) * Speed * Time.deltaTime;
                var newPos = HeroFollowPoint.localPosition + deltaPos;
                newPos.x = Mathf.Clamp(newPos.x, -Edge, Edge);
                HeroFollowPoint.localPosition = newPos;
            }

            // process shooting
            CurrentShootReloadingTime += Time.deltaTime;
            if (isShooting && CurrentShootReloadingTime > ShootDelay)
            {
                CurrentShootReloadingTime = 0f;
                SpawnRocket();
            }
        }

        public void Freeze(bool flag)
        {
            enabled = !flag;
        }

        private void SpawnRocket()
        {
            var rocket = Factory.Create("RocketRed", _gameProcessor.Projectiles,
                Ship.transform.position, RocketLifeTime).GetComponent<Rocket>();
            rocket.Enemies = _gameProcessor.Monsters;
            rocket.Damage = Damage;
        }

        private void SpawnShip()
        {
            Ship = Instantiate(PrefabHolder.Instance.Entities["Ship"], HeroSpawnPoint) as GameObject;
            Assert.IsNotNull(Ship);
            HeroFollowPoint.localPosition = new Vector3(0, HeroFollowPoint.localPosition.y, HeroFollowPoint.localPosition.z);
            Ship.GetComponent<Follower>().SetTarget(HeroFollowPoint);
        }

        public Vector3 GetHeroApproxPosition()
        {
            return HeroFollowPoint.position;
        }

        public bool IsDead()
        {
            return Ship == null;
        }

        public void ExplodeShip()
        {
            if (IsDead())
                return;
            StartCoroutine(ExplodingShip());
        }

        private IEnumerator ExplodingShip()
        {
            // boom 
            Factory.Create("Explosion", _gameProcessor.Effects, Ship.transform.position, ExplosionDelay);
            Factory.Create("PfxBoom", _gameProcessor.Effects, Ship.transform.position, ExplParticlesDuration);
            Destroy(Ship);

            yield return new WaitForSeconds(RespawnDelay);

            _gameProcessor.Lives--;
            if (_gameProcessor.Lives >= 0)
                SpawnShip();
        }
    }
}
