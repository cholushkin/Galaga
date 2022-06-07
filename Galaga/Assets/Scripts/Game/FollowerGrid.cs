using UnityEngine;

namespace Galaga.Game
{
    public class FollowerGrid : MonoBehaviour
    {
        public int GridX;
        public int GridY;
        public Grid Target;

        public float smoothTime = 0.3F;
        private Vector3 velocity = Vector3.zero;

        void Update()
        {
            if (Target == null)
                return;
            transform.position = Vector3.SmoothDamp(transform.position, Target.Get(GridX, GridY), ref velocity, smoothTime);
        }
    }
}
