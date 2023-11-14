using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public enum EAudioChannelMode
    {
        Multiple,
        Exclusive
    }

    public class AudioChannel : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private EAudioChannelMode m_audioChannelMode;
        [SerializeField] private AudioSource m_audioSource = null;

        [Header("Exclusive Mode Settings")]
        [SerializeField] private float m_fadeOutDuration = 0.5f;
        [SerializeField] private float m_fadeInDuration = 0.25f;

        private Coroutine m_transitionCoroutine = null;
        private AudioClipResolver m_lastPlayedClip = null;
        private float m_initialVolume = 1.0f;

        public AudioClipResolver lastPlayedAudioClipResolver => m_lastPlayedClip;

        private void Awake()
        {
            m_initialVolume = m_audioSource.volume;
        }

        public void Play(AudioClipResolver audioClipResolver)
        {
            AudioClip audioClip = audioClipResolver.GetClip();
            m_lastPlayedClip = audioClipResolver;

            if (audioClip != null)
            {
                if (m_audioChannelMode == EAudioChannelMode.Exclusive)
                {
                    if (m_transitionCoroutine != null)
                    {
                        StopCoroutine(m_transitionCoroutine);
                    }

                    m_transitionCoroutine = StartCoroutine(FadeOutAndIn(audioClip));
                }
                else
                {
                    m_audioSource.PlayOneShot(audioClip);
                }
            }
        }

        public IEnumerator FadeOutAndIn(AudioClip newClip)
        {
            // Fade out
            while (m_audioSource.volume > 0)
            {
                m_audioSource.volume -= m_initialVolume * Time.unscaledDeltaTime / m_fadeOutDuration;
                yield return null;
            }
            m_audioSource.Stop();
            m_audioSource.clip = newClip;
            m_audioSource.Play();

            // Fade in
            while (m_audioSource.volume < m_initialVolume)
            {
                m_audioSource.volume += m_initialVolume * Time.unscaledDeltaTime / m_fadeInDuration;
                yield return null;
            }
            m_audioSource.volume = m_initialVolume;
        }
    }
}
