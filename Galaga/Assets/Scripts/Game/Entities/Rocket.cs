using UnityEngine;

namespace Galaga.Game
{
    public class Rocket : MonoBehaviour
    {
        public float Speed;
        public float Damage;
        public Transform Enemies { get; set; }

        void Update()
        {
            var delta = Speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + delta, transform.position.z);

            ProcessCollisions();
        }

        // note: for 50-100 units is still ok, less than 0.5% of CPU
        private void ProcessCollisions()
        {
            const float CollisionFactor = 0.25f;
            var enemies = Enemies.GetComponentsInChildren<BaseEnemy>();
            foreach (var enemy in enemies)
            {
                var distance = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distance < CollisionFactor)
                {
                    enemy.ReceiveDamage(Damage);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}
