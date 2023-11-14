using System;
using System.Collections;

namespace Gyvr.Mythril2D
{
    public static class CoroutineHelpers
    {
        public static IEnumerator ExecuteInXFrames(int frames, Action callback)
        {
            for (int i = 0; i < frames; ++i)
            {
                yield return null;
            }

            callback();
        }
    }
}
