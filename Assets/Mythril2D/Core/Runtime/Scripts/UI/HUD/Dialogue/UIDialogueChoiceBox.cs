using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class UIDialogueChoiceBox : MonoBehaviour
    {
        // Inspector Settings
        [SerializeField] private UIDialogueOption[] m_options = null;

        private void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void SetOptions(string[] options)
        {
            for (int i = 0; i < m_options.Length; ++i)
            {
                UIDialogueOption option = m_options[i];

                if (i < options.Length)
                {
                    option.SetText(options[i]);
                    option.SetVisible(true);
                }
                else
                {
                    option.SetVisible(false);
                }
            }
        }

        public string[] GetOptionNames(DialogueNodeOption[] options)
        {
            string[] output = new string[options.Length];

            for (int i = 0; i < options.Length; ++i)
            {
                output[i] = options[i].name;
            }

            return output;
        }

        public void Show(DialogueNodeOption[] options)
        {
            SetVisible(true);
            SetOptions(GetOptionNames(options));
            GameManager.EventSystem.SetSelectedGameObject(m_options[0].gameObject);
        }

        public void Hide()
        {
            SetVisible(false);
        }
    }
}
