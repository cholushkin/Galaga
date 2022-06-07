using UnityEngine;

namespace Galaga.Game
{
    public class RocketBlue : MonoBehaviour
    {
        public float Speed;
        public HeroController HeroController { get; set; }

        void Update()
        {
            var delta = Speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - delta, transform.position.z);
            ProcessCollisions();
        }

        private void ProcessCollisions()
        {
            const float CollisionFactor = 0.25f;
            var ship = HeroController.Ship;
            if (ship == null)
                return;
            var distance = (ship.transform.position - transform.position).sqrMagnitude;
            if (distance < CollisionFactor)
            {
                HeroController.ExplodeShip();
                Destroy(gameObject);
            }
        }
    }
}
