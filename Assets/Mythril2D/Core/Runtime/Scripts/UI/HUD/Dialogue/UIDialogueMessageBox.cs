using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIDialogueMessageBox : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private string m_animationParameter = "visible";

        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_dialogueBlipAudio;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_text = null;
        [SerializeField] private UIDialogueSpeakerBox m_speakerBox = null;
        [SerializeField] private GameObject m_arrow = null;


        // Public Members
        private Queue<char> m_charQueue = null;

        // Private Members
        private bool m_hasAnimationParameter = false;
        private bool m_visible = false;
        private bool m_textAnimationInProgress = false;
        private bool m_showArrow = false;

        // Component References
        private Animator m_animator = null;

        public void Show() => SetVisible(true);
        public void Hide() => SetVisible(false);

        private void Awake()
        {
            m_animator = GetComponent<Animator>();

            if (m_animator)
            {
                m_hasAnimationParameter = AnimationUtils.HasParameter(m_animator, m_animationParameter);
            }
        }

        public void SetText(string speaker, string text, bool showArrow)
        {
            m_speakerBox.SetText(speaker);
            m_showArrow = showArrow;
            m_text.text = "";
            m_charQueue = new Queue<char>(text);
            StartCoroutine(UpdateText());
        }

        public void SetMargin(Vector4 margins)
        {
            m_text.margin = margins;
        }

        IEnumerator UpdateText()
        {
            OnTextAnimationStart();

            while (m_charQueue != null && m_charQueue.Count > 0)
            {
                char c = m_charQueue.Dequeue();

                m_text.text += c;

                if (!char.IsWhiteSpace(c))
                {
                    GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_dialogueBlipAudio);
                }

                yield return new WaitForSecondsRealtime(0.05f);
            }

            OnTextAnimationFinished();
        }

        private void OnTextAnimationStart()
        {
            m_textAnimationInProgress = true;
            m_arrow.SetActive(false);
        }

        private void OnTextAnimationFinished()
        {
            if (m_showArrow)
            {
                m_arrow.SetActive(true);
            }

            m_textAnimationInProgress = false;

            SendMessageUpwards("OnMessageBoxTextAnimationFinished");
        }

        public void SkipTextAnimation()
        {
            StopCoroutine(UpdateText());
            StartCoroutine(CoroutineHelpers.ExecuteInXFrames(1, OnTextAnimationFinished));
            m_text.text += new string(m_charQueue.ToArray());
            m_charQueue.Clear();
        }

        public bool IsTextAnimationFinished()
        {
            return !m_textAnimationInProgress;
        }

        private void SetVisible(bool visible)
        {
            if (visible != m_visible)
            {
                m_visible = visible;

                if (m_animator && m_hasAnimationParameter)
                {
                    m_animator.SetBool(m_animationParameter, visible);
                }
            }
        }
    }
}
