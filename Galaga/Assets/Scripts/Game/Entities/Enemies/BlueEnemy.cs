namespace Galaga.Game
{
    public class BlueEnemy : BaseEnemy
    {
        protected override void Awake()
        {
            base.Awake();
            SetState(State.StayOnGrid);
        }


        protected virtual void Update()
        {
            const float StartFlyDownDistance = 4f;
            const float CheckFlydownDistance = 1f;


            // process collision
            if (_state == State.FlyToHero || _state == State.FlyToBottom)
            {
                if (ProcessCollisionWithHero())
                    _gameProcessor.HeroController.ExplodeShip();
            }

            // process states
            if (_state == State.FlyToHero)
            {
                LookToHero();
                if (_follower.VerticalDistance < StartFlyDownDistance) 
                {
                    SetState(State.FlyToBottom);
                    StartFlyToBottom();
                }
            }
            else if (_state == State.FlyToBottom)
                if (_follower.VerticalDistance < CheckFlydownDistance)
                    RespawnOnTop();
        }


        public override void StartThink()
        {
            if (_state != State.StayOnGrid)
                return;

            SetState(State.FlyToHero);
            StartFlyToHeroLastPos();
        }
    }
}
