using Galaga.System;
using UnityEngine.Assertions;

namespace Galaga.GUI
{
    public class SimpleGUI : Singleton<SimpleGUI>
    {
        public interface IGUIScreen
        {
            void StartAppearAnimation();
            void StartDisappearAnimation();
            string GetName();
        }

        public IGUIScreen PeekScreen() // last active child
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                var disScreen = transform.GetChild(i).gameObject;
                if (disScreen.activeSelf)
                    return disScreen.GetComponent<GUIScreenBase>();
            }
            return null;
        }

        public IGUIScreen PopScreen() // disable last active child
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                var disScreen = transform.GetChild(i).gameObject;
                if (disScreen.activeSelf)
                {
                    var screen = disScreen.GetComponent<GUIScreenBase>();
                    screen.StartDisappearAnimation();
                    return screen;
                }
            }
            return null;
        }

        public void PushScreen(string screenName)
        {
            // find screen in children of current SimpleGUI
            var screenTr = transform.Find(screenName);
            Assert.IsNotNull(screenTr);
            var screen = screenTr.GetComponent<GUIScreenBase>();
            Assert.IsNotNull(screen);

            // push it on the top of the tree
            screenTr.SetAsLastSibling();
            screen.StartAppearAnimation();
        }
    }
}