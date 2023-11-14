using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public struct FloatingTextAnimation
    {
        public string text;
        public Vector2 position;
        public Color color;
        public string animationTrigger;
    }

    public class FloatingTextPool : MonoBehaviour
    {
        // Inspector Settings
        [Header("References")]
        [SerializeField] private GameObject m_floatingTextPrefab = null;

        [Header("Settings")]
        [SerializeField] private int m_poolSize = 3;
        [SerializeField] private float m_minimumDelayBetweenTexts = 0.15f;

        // Private Members
        private float m_cooldown = 0.0f;
        private FloatingText[] m_floatingTexts = null;
        private Queue<FloatingTextAnimation> m_queue = null;

        private void Awake()
        {
            m_floatingTexts = new FloatingText[m_poolSize];
            m_queue = new Queue<FloatingTextAnimation>();

            for (int i = 0; i < m_poolSize; ++i)
            {
                GameObject instance = Instantiate(m_floatingTextPrefab, Vector3.zero, Quaternion.identity, transform);
                FloatingText floatingText = instance.GetComponent<FloatingText>();

                instance.name = "FloatingText_" + i;

                instance.SetActive(false);

                if (floatingText)
                {
                    m_floatingTexts[i] = floatingText;
                }
                else
                {
                    Debug.LogError("FloatingText Prefab invalid. Make sure the prefab has a FloatingText component");
                }
            }
        }

        private void Update()
        {
            if (m_queue.Count > 0 && m_cooldown == 0.0f)
            {
                FloatingTextAnimation animation = m_queue.Peek();

                FloatingText floatingText = FindAvailableText();

                if (floatingText)
                {
                    floatingText.Play(animation.text, animation.position, animation.color, animation.animationTrigger);
                    m_queue.Dequeue();
                    m_cooldown = m_minimumDelayBetweenTexts;
                }
                else
                {
                    Debug.LogError("No floating text available. Consider expanding pool");
                }
            }

            m_cooldown = Math.Max(0.0f, m_cooldown - Time.deltaTime);
        }

        private FloatingText FindAvailableText()
        {
            return Array.Find(m_floatingTexts, (element) => !element.isActiveAndEnabled);
        }

        public void ShowText(string text, Vector2 position, Color color, string animationTrigger)
        {
            m_queue.Enqueue(new FloatingTextAnimation
            {
                text = text,
                position = position,
                color = color,
                animationTrigger = animationTrigger
            });
        }
    }
}
