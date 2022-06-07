using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Galaga.System
{
    public class AppStateManager : Singleton<AppStateManager>
    {
        public abstract class IAppState : MonoBehaviour
        {
            public abstract void AppStateEnter(bool animated);
            public abstract void AppStateLeave(bool animated);
        }

        public IAppState StartState;
        public IAppState[] States;

        private IAppState _currentMode;

        void Start()
        {
            GoToState(StartState.GetType(), false);
        }

        public void GoToState(Type state, bool animated)
        {
            if (null != _currentMode && _currentMode.GetType() == state)
                return;

            var next = States.FirstOrDefault(s => s.GetType() == state);
            Assert.IsNotNull(next);

            // hope StateLeave won't call Start
            if (null != _currentMode)
                _currentMode.AppStateLeave(animated);

            _currentMode = next;
            _currentMode.AppStateEnter(animated);
        }
    }
}

