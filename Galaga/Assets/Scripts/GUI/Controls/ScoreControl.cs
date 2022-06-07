using Galaga.Game;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Galaga.GUI
{
    public class ScoreControl : MonoBehaviour
    {
        private GameProcessor _gameProcessor;
        private Text _text;

        void Awake()
        {
            _text = GetComponent<Text>();
        }

        void RefreshScore()
        {
            _text.text = _gameProcessor.Scores.ToString();
        }

        public void Subscribe(GameProcessor gameProcessor)
        {
            Assert.IsNotNull(gameProcessor);
            Debug.Log("ScoreControl:Subscribe: to new gameProcessor " + gameProcessor.GetHashCode());
            _gameProcessor = gameProcessor;
            gameProcessor.ScoreChanged += RefreshScore;
        }
    }
}
