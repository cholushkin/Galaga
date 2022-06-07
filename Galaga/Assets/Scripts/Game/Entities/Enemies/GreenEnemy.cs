using UnityEngine;

namespace Galaga.Game
{
    public class GreenEnemy : BaseEnemy
    {
        private GameObject EnergyBeam;
        private bool _isLockHero;

        public override void StartThink()
        {
            if (_state != State.StayOnGrid)
                return;

            SetState(State.FlyToHero);
            StartFlyToHeroFront();
        }

        protected virtual void Update()
        {
            const float CheckFlydownDistance = 1f;

            // process collision
            if (_state == State.FlyToBottom)
                if (ProcessCollisionWithHero())
                    _gameProcessor.HeroController.ExplodeShip();

            // process states
            if (_state == State.FlyToHero)
            {
                if (_follower.VerticalDistance < CheckFlydownDistance)
                {
                    SetState(State.Waiting);
                    EnergyBeam = Factory.Create("EnergyBeem", transform, transform.position, Weapon);
                }
            }
            else if (_state == State.Waiting)
            {
                if (EnergyBeam == null) // stop shooting
                {
                    SetState(State.FlyToBottom);
                    StartFlyToBottom();
                    _isLockHero = false;
                    _gameProcessor.HeroController.Freeze(false);
                }
                else
                {
                    if (_isLockHero)
                        return;
                    var ship = _gameProcessor.HeroController.Ship;
                    if (ship != null &&
                        EnergyBeam.GetComponent<SpriteRenderer>().bounds.Contains(ship.transform.position))
                    {
                        _isLockHero = true;
                        _gameProcessor.HeroController.Freeze(true);
                    }
                }
            }
            else if (_state == State.FlyToBottom)
                if (_follower.VerticalDistance < CheckFlydownDistance)
                    RespawnOnTop();
        }

        public override void OnBeforeDie()
        {
            if (_isLockHero)
                _gameProcessor.HeroController.Freeze(false);
        }
    }
}