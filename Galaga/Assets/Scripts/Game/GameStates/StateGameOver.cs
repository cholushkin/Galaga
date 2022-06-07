using Galaga.GUI;
using Galaga.System;
using UnityEngine;

namespace Galaga.Game
{
    public class StateGameOver : AppStateManager.IAppState
    {
        #region From IAppState
        public override void AppStateEnter(bool animated)
        {
            Debug.Log("Enter StateGameOver state");
            SimpleGUI.Instance.PushScreen("Screen.GameOver");
            gameObject.SetActive(true);
        }

        public override void AppStateLeave(bool animated)
        {
            Debug.Log("Leave StateGameOver state");
            gameObject.SetActive(false);
            SimpleGUI.Instance.PopScreen();
        }
        #endregion
    }
}