using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UIHUDAbilityBarEntry : UIAbility
    {
        [Header("References")]
        [SerializeField] private UIControllerButton m_controllerButton = null;

        public override void SetAbility(AbilitySheet sheet, int index)
        {
            base.SetAbility(sheet, index);

            // Hide empty abilities in the HUD
            gameObject.SetActive(sheet);

            if (sheet)
            {
                m_controllerButton.SetAction((UIControllerButtonManager.EAction)(index + (int)UIControllerButtonManager.EAction.FireAbility1));
            }
        }
    }
}
