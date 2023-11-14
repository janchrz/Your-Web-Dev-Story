using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gyvr.Mythril2D
{
    public class MapLoadingDelegationParams
    {
        public Action<Action> unloadDelegate;
        public Action<Action> loadDelegate;
        public Action completionDelegate;
    }

    public class MapLoadingSystem : AGameSystem
    {
        [Header("Settings")]
        [SerializeField] private bool m_delegateTransitionResponsability = false;

        private string m_currentMap = string.Empty;

        public void SetActiveMap(string map)
        {
            m_currentMap = map;
        }

        public string GetCurrentMapName()
        {
            return m_currentMap;
        }

        public bool HasCurrentMap()
        {
            return m_currentMap != string.Empty;
        }

        public void RequestTransition(string map, Action onMapUnloaded = null, Action onMapLoaded = null, Action onCompletion = null)
        {
            GameManager.NotificationSystem.mapTransitionStarted.Invoke();

            if (m_delegateTransitionResponsability)
            {
                DelegateTransition(map, onMapUnloaded, onMapLoaded, onCompletion);
            }
            else
            {
                ExecuteTransition(map, onMapUnloaded, onMapLoaded, onCompletion);
            }
        }

        private void DelegateTransition(string map, Action onMapUnloaded = null, Action onMapLoaded = null, Action onCompletion = null)
        {
            GameManager.NotificationSystem.mapTransitionDelegationRequested.Invoke(new MapLoadingDelegationParams
            {
                unloadDelegate = HasCurrentMap() ? (callback) => UnloadMap(m_currentMap, callback + onMapUnloaded) : null,
                loadDelegate = (callback) => LoadMap(map, callback + onMapLoaded),
                completionDelegate = () => CompleteTransition(onCompletion)
            });
        }

        private void ExecuteTransition(string map, Action onMapUnloaded = null, Action onMapLoaded = null, Action onCompletion = null)
        {
            Action loadAction =
                () => LoadMap(map,
                (() => CompleteTransition(onCompletion)) + onMapLoaded);

            if (HasCurrentMap())
            {
                UnloadMap(m_currentMap, loadAction);
            }
            else
            {
                loadAction();
            }
        }

        private void UnloadMap(string map, Action onCompletion)
        {
            if (map != string.Empty && map == m_currentMap)
            {
                Debug.Log($"Unloading Map {map}...");

                AsyncOperation operation = SceneManager.UnloadSceneAsync(map);

                operation.completed += (op) =>
                {
                    GameManager.NotificationSystem.mapUnloaded.Invoke();
                    onCompletion?.Invoke();
                };
            }
            else
            {
                GameManager.NotificationSystem.mapUnloaded.Invoke();
                onCompletion?.Invoke();
            }
        }

        private void LoadMap(string map, Action onCompletion)
        {
            if (map != string.Empty && map != m_currentMap)
            {
                Debug.Log($"Loading Map {map}...");

                AsyncOperation operation = SceneManager.LoadSceneAsync(map, LoadSceneMode.Additive);

                operation.completed += (op) =>
                {
                    SetActiveMap(map);
                    GameManager.NotificationSystem.mapLoaded.Invoke();
                    onCompletion?.Invoke();
                };
            }
            else
            {
                GameManager.NotificationSystem.mapLoaded.Invoke();
                onCompletion?.Invoke();
            }
        }

        private void CompleteTransition(Action onCompletion)
        {
            GameManager.NotificationSystem.mapTransitionCompleted.Invoke();
            onCompletion?.Invoke();
        }
    }
}
