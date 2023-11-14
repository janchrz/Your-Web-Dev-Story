using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIControllerButton : MonoBehaviour
    {
        [SerializeField] private Image m_image = null;
        [SerializeField] private SpriteRenderer m_spriteRenderer = null;
        [SerializeField] private UIControllerButtonManager.EAction m_action;

        private Dictionary<UIControllerButtonManager.EButtonState, Sprite> m_sprites;
        private InputAction m_inputAction = null;

        public UIControllerButtonManager.EAction action => m_action;

        private void Start()
        {
            UIControllerButtonManager.RegisterButton(this);
        }

        private void OnDestroy()
        {
            UIControllerButtonManager.UnregisterButton(this);
        }

        private void Update()
        {
            if (m_sprites != null)
            {
                Sprite sprite = m_sprites[GetCurrentButtonState()];

                if (m_image) m_image.sprite = sprite;
                if (m_spriteRenderer) m_spriteRenderer.sprite = sprite;
            }
        }

        private UIControllerButtonManager.EButtonState GetCurrentButtonState()
        {
            if (m_inputAction != null)
            {
                return
                    m_inputAction.IsInProgress() ?
                    UIControllerButtonManager.EButtonState.Pressed :
                    UIControllerButtonManager.EButtonState.Idle;
            }

            return UIControllerButtonManager.EButtonState.Idle;
        }

        public void Initialize(Dictionary<UIControllerButtonManager.EButtonState, Sprite> sprites, InputAction inputAction = null)
        {
            m_sprites = sprites;
            m_inputAction = inputAction;
        }

        public void SetAction(UIControllerButtonManager.EAction action)
        {
            m_action = action;
            UIControllerButtonManager.ForceUpdateButton(this);
        }
    }
}
