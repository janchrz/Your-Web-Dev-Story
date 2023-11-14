using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class ConditionalReferencesActivator : AConditionalActivator
    {
        [SerializeField] private GameObject[] m_references = null;

        protected override void Activate(bool state)
        {
            foreach (GameObject reference in m_references)
            {
                reference.SetActive(state);
            }
        }
    }
}
