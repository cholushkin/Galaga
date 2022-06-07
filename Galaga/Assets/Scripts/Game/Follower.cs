using UnityEngine;

namespace Galaga.Game
{
    public class Follower : MonoBehaviour
    {
        public float smoothTime = 0.3F;

        public float Distance { get; private set; }
        public float VerticalDistance { get; private set; }
        private Vector3 velocity = Vector3.zero;
        private Transform Target;
        private Vector3? TargetV3;

        Follower()
        {
            Distance = float.PositiveInfinity;
            VerticalDistance = float.PositiveInfinity;
        }

        public void SetTarget(Transform targetTransform)
        {
            Target = targetTransform;
            TargetV3 = null;
            Distance = (Target.position - transform.position).magnitude;
            VerticalDistance = Mathf.Abs((Target.position - transform.position).y);
        }

        public void SetTarget(Vector3 v3Target)
        {
            Target = null;
            TargetV3 = v3Target;
            Distance = (v3Target - transform.position).magnitude;
            VerticalDistance = Mathf.Abs((v3Target - transform.position).y);
        }

        void Update()
        {
            if (Target == null && TargetV3 == null)
                return;
            var trg = (TargetV3 == null) ? Target.position : TargetV3.Value;
            transform.position = Vector3.SmoothDamp(transform.position, trg, ref velocity, smoothTime);
            Distance = (trg - transform.position).magnitude;
            VerticalDistance = Mathf.Abs((trg - transform.position).y);
        }
    }
}
