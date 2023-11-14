using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIPlayerControllerFeedback : MonoBehaviour
    {
        [SerializeField] private UIControllerButton m_interactionButtonFeedback = null;
        [SerializeField] private SpriteRenderer m_spriteRenderer = null;
        [SerializeField] private Vector3 m_offset = Vector3.up;
        [SerializeField] private float m_showAnimationSpeed = 20.0f;
        [SerializeField] private float m_hideAnimationSpeed = 20.0f;

        private Color m_initialSpriteColor = Color.white;

        private PlayerController m_playerController = null;

        private void Start()
        {
            m_playerController = GameManager.PlayerSystem.PlayerInstance.GetComponent<PlayerController>();
            m_initialSpriteColor = m_spriteRenderer.color;
        }

        private void Update()
        {
            GameObject target = m_playerController.interactionTarget;

            if (target)
            {
                if (!m_interactionButtonFeedback.isActiveAndEnabled)
                {
                    m_interactionButtonFeedback.gameObject.SetActive(true);
                }

                m_interactionButtonFeedback.transform.position = target.transform.position + m_offset;
                m_spriteRenderer.color = Color.Lerp(m_spriteRenderer.color, m_initialSpriteColor, m_showAnimationSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                m_spriteRenderer.color = Color.Lerp(m_spriteRenderer.color, new Color(1.0f, 1.0f, 1.0f, 0.0f), m_hideAnimationSpeed * Time.unscaledDeltaTime);
            }
        }
    }
}
