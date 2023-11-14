using UnityEngine;

namespace Gyvr.Mythril2D
{
    public interface IUIMenu
    {
        public void Init();
        public void OnMenuPushed() { }
        public void OnMenuPopped() { }
        public void Show(params object[] args);
        public GameObject FindSomethingToSelect();
        public void Hide();
        public void EnableInteractions(bool enable);
    }
}
