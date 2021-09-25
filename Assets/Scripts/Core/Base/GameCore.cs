using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class GameCore : MonoBehaviour
    {
        public static EventMgr Event;
        public static GameCore Self;

        public static Action<bool> OnAppPause;
        public static Action<bool> OnAppFocus;
        public static Action OnAppQuit;

        protected abstract void OnModeStart();
        protected abstract void OnModeUpdate();
        protected abstract void OnModeFixedUpdate();
        protected abstract void OnModeDestroy();

        private IEnumerator Start()
        {
            GameCore.Self = this;

            #region module

            Event = ModuleMgr.GetModule<EventMgr>();

            #endregion

            #region resource

            #endregion

            #region state

            #endregion

            #region audio

            #endregion

            ModuleMgr.Init();
            OnModeStart();
            yield return new WaitForEndOfFrame();
        }

        private void Update()
        {
            ModuleMgr.Update();
            OnModeUpdate();
        }

        private void FixedUpdate()
        {
            ModuleMgr.FixedUpdate();
            OnModeFixedUpdate();
        }

        private void OnDestroy()
        {
            ModuleMgr.ShutDown();
            OnModeDestroy();
        }

        private void OnGUI()
        {
            ModuleMgr.ImGui();
        }

        private void OnApplicationPause(bool pause)
        {
            OnAppPause?.Invoke(pause);
        }

        private void OnApplicationFocus(bool focus)
        {
            OnAppFocus?.Invoke(focus);
        }

        private void OnApplicationQuit()
        {
            OnAppQuit?.Invoke();
        }
    }
}