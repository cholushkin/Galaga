using Galaga.Game;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.GUI
{
    public class LivesControl : MonoBehaviour
    {
        private GameProcessor _gameProcessor;
        private int _currentLives;

        void RefreshLives()
        {
            var delta = _gameProcessor.Lives - _currentLives;

            if (delta > 0) // need add lives
                for (int i = 0; i < delta; ++i)
                    Factory.Create("Life", transform, Factory.Group.Topology);

            if (delta < 0) // need remove lives
                foreach (Transform children in transform)
                {
                    Destroy(children.gameObject);
                    if (++delta == 0)
                        break;
                }
            _currentLives = _gameProcessor.Lives;
        }

        public void Subscribe(GameProcessor gameProcessor)
        {
            Assert.IsNotNull(gameProcessor);
            Debug.Log("LivesControl:Subscribe: to new gameProcessor " + gameProcessor.GetHashCode());
            _gameProcessor = gameProcessor;
            _currentLives = gameProcessor.Lives;
            gameProcessor.LivesChanged += RefreshLives;
        }
    }
}
