using Galaga.GUI;
using Galaga.System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.Game
{

    public class StateGameplay : AppStateManager.IAppState
    {
        private GameProcessor _gameProcessor;

        void Update()
        {
            if (_gameProcessor.IsGameOver)
            {
                Debug.Log("Game over");
                ProcessTopScores();
                AppStateManager.Instance.GoToState(typeof(StateGameOver), false);
            }
        }

        private void ProcessTopScores()
        {
            PlayerPrefs.SetInt("LastScores", _gameProcessor.Scores);
            if (PlayerPrefs.GetInt("TopScores") < _gameProcessor.Scores)
                PlayerPrefs.SetInt("TopScores", _gameProcessor.Scores);
        }

        #region From IAppState
        public override void AppStateEnter(bool animated)
        {
            Debug.Log("Enter StateGameplay");

            // create game
            var game = Factory.Create("Game", transform, Factory.Group.Topology);
            _gameProcessor = game.GetComponent<GameProcessor>();

            Assert.IsNotNull(game);
            Assert.IsNotNull(_gameProcessor);

            // set HUD
            SimpleGUI.Instance.PushScreen("Screen.HUD");

            // subscribe HUD to the new game
            var ScreenHUD = SimpleGUI.Instance.PeekScreen() as ScreenHUD;
            Assert.IsNotNull(ScreenHUD);
            ScreenHUD.ScoresCtrl.Subscribe(_gameProcessor);
            ScreenHUD.LivesControl.Subscribe(_gameProcessor);

            gameObject.SetActive(true);
        }

        public override void AppStateLeave(bool animated)
        {
            gameObject.SetActive(false);
            Debug.Log("Leave StateGameplay state");
            Destroy(_gameProcessor.gameObject);
            SimpleGUI.Instance.PopScreen();
        }
        #endregion
    }

}
