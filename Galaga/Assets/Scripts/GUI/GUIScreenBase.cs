using UnityEngine;


namespace Galaga.GUI
{
    public class GUIScreenBase : MonoBehaviour, SimpleGUI.IGUIScreen
    {
        protected SimpleGUI _simpleGui;

        void Start()
        {
            _simpleGui = GetComponentInParent<SimpleGUI>();
        }

        public virtual void StartAppearAnimation()
        {
            gameObject.SetActive(true);
        }

        public virtual void StartDisappearAnimation()
        {
            gameObject.SetActive(false);
        }

        public string GetName()
        {
            return gameObject.name;
        }
    }
}
