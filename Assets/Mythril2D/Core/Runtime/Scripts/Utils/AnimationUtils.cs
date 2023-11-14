using UnityEngine;

namespace Gyvr.Mythril2D
{
    public static class AnimationUtils
    {
        public static bool HasParameter(Animator animator, string parameter)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter)
                {
                    return true;
                }
            }

            return false;
        }
    }
}