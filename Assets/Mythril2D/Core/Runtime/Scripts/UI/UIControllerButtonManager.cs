using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

namespace Gyvr.Mythril2D
{
    public class UIControllerButtonManager : MonoBehaviour
    {
        public enum EControllerType
        {
            Keyboard,
            XBOX,
            Playstation
        }

        public enum EAction
        {
            Interact,
            FireAbility1,
            FireAbility2,
            FireAbility3,
            OpenGameMenu,
            Submit,
            Cancel
        }

        public enum EButtonState
        {
            Idle,
            Pressed
        }

        [SerializeField] private SerializableDictionary<EControllerType, SpriteLibraryAsset> m_controllerSpriteLibraries = null;

        private HashSet<UIControllerButton> m_buttons = new HashSet<UIControllerButton>();
        private EControllerType m_controllerType;
        private static UIControllerButtonManager _instance = null;

        public static void RegisterButton(UIControllerButton button) => _instance?.InternalButtonRegistration(button);
        public static void UnregisterButton(UIControllerButton button) => _instance?.InternalButtonUnregistration(button);
        public static void ForceUpdateButton(UIControllerButton button) => _instance?.UpdateButton(button);

        private void Awake()
        {
            Debug.Assert(_instance == null, "An instance of UIControllerButtonManager already exists");
            _instance = this;
        }

        private void Start()
        {
            UpdateControllerType(GameManager.InputSystem.playerInput);
            GameManager.InputSystem.playerInput.onControlsChanged += UpdateControllerType;
        }

        private void OnDestroy()
        {
            GameManager.InputSystem.playerInput.onControlsChanged -= UpdateControllerType;
            _instance = null;
        }

        private void UpdateControllerType(PlayerInput playerInput)
        {
            string devices = string.Empty;

            foreach (var device in playerInput.devices)
            {

                if (device.enabled)
                {
                    devices += device.name + ";";
                }
            }

            devices = devices.ToLower();

            if (devices.Contains("xinput"))
            {
                SetControllerType(EControllerType.XBOX);
            }
            else if (devices.Contains("dualsense"))
            {
                SetControllerType(EControllerType.Playstation);
            }
            else if (devices.Contains("keyboard"))
            {
                SetControllerType(EControllerType.Keyboard);
            }
        }

        private void InternalButtonRegistration(UIControllerButton button)
        {
            m_buttons.Add(button);
            UpdateButton(button);
        }

        private void InternalButtonUnregistration(UIControllerButton button)
        {
            m_buttons.Remove(button);
        }

        private void SetControllerType(EControllerType controllerType)
        {
            Debug.Log($"Controller set to {controllerType}");
            m_controllerType = controllerType;
            UpdateControllerButtons();
        }

        private void UpdateControllerButtons()
        {
            foreach (UIControllerButton button in m_buttons)
            {
                UpdateButton(button);
            }
        }

        private InputAction GetInputAction(EAction action)
        {
            switch (action)
            {
                case EAction.Interact: return GameManager.InputSystem.gameplay.interact;
                case EAction.FireAbility1: return GameManager.InputSystem.gameplay.fireAbility1;
                case EAction.FireAbility2: return GameManager.InputSystem.gameplay.fireAbility2;
                case EAction.FireAbility3: return GameManager.InputSystem.gameplay.fireAbility3;
                case EAction.OpenGameMenu: return GameManager.InputSystem.gameplay.openGameMenu;
            }

            return null;
        }

        public void UpdateButton(UIControllerButton button)
        {
            SpriteLibraryAsset currentLibrary = m_controllerSpriteLibraries[m_controllerType];

            Dictionary<EButtonState, Sprite> sprites = new Dictionary<EButtonState, Sprite>();

            foreach (EButtonState state in Enum.GetValues(typeof(EButtonState)).Cast<EButtonState>())
            {
                Sprite sprite = currentLibrary.GetSprite(button.action.ToString(), state.ToString());
                sprites.Add(state, sprite);
            }

            // TODO: Pass in a ButtonControl here
            button.Initialize(sprites, GetInputAction(button.action));
        }
    }
}
