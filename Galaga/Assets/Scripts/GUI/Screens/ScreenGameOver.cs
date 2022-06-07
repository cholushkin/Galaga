using Galaga.Game;
using Galaga.System;
using UnityEngine;
using UnityEngine.UI;

namespace Galaga.GUI
{
    public class ScreenGameOver : GUIScreenBase
    {
        public Text HighScore;
        public Text TopScore;
        public GameObject NewTopScore;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AppStateManager.Instance.GoToState(typeof(StateGameplay), false);
        }

        public override void StartAppearAnimation()
        {
            var lastScores = PlayerPrefs.GetInt("LastScores");
            var topScores = PlayerPrefs.GetInt("TopScores");
            HighScore.text = lastScores.ToString();
            TopScore.text = topScores.ToString();
            NewTopScore.SetActive(lastScores >= topScores);
            base.StartAppearAnimation();
        }
    }
}
