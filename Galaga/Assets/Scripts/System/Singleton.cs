using UnityEngine;

namespace Galaga.System
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        public static void KillGameObject()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }
        }

        protected virtual void Awake()
        {
            Debug.Log("Singleton instance assigning: " + GetType());
            if (Instance != null)
                Debug.LogError("Got a second instance of the class " + GetType());
            Instance = (T)this;
        }
    }
}
