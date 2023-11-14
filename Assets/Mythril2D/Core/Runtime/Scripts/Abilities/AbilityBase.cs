using UnityEngine;

namespace Gyvr.Mythril2D
{
    public abstract class AbilityBase : MonoBehaviour
    {
        public abstract AbilitySheet abilitySheet { get; }

        protected CharacterBase m_character = null;

        public virtual void Init(CharacterBase character, AbilitySheet settings)
        {
            m_character = character;
        }
    }
}
