using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class AudioRegion : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected AudioClipResolver m_audioClipResolver = null;

        private AudioClipResolver m_previousAudio = null;

        public bool IsPlayer(Collider2D collision)
        {
            if (GameManager.Player && GameManager.Player.gameObject)
            {
                return collision.gameObject == GameManager.Player.gameObject;
            }

            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsPlayer(collision))
            {
                m_previousAudio = GameManager.AudioSystem.GetLastPlayedAudioClipResolver(m_audioClipResolver.targetChannel);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_audioClipResolver);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsPlayer(collision))
            {
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_previousAudio);
            }
        }
    }
}