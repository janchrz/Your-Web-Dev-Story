using UnityEngine;

namespace Gyvr.Mythril2D
{
    public abstract class AGameSystem : MonoBehaviour
    {
        public virtual void OnSystemInit() { }
        public virtual void OnSystemStart() { }
        public virtual void OnSystemStop() { }

        public virtual void OnMapLoaded() { }
        public virtual void OnMapUnloaded() { }
    }
}
