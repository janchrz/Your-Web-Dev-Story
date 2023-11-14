using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_ScriptableActions + nameof(AudioPlaybackAction))]
    public class AudioPlaybackAction : ScriptableAction
    {
        public override Type[] RequiredArgs => new Type[] { typeof(AudioClipResolver) };
        public override Type[] OptionalArgs => new Type[] { };
        public override string[] ArgDescriptions => new string[] { "Clip to play" };

        public override void Execute(params object[] args)
        {
            Play(GetRequiredArgAtIndex<AudioClipResolver>(0, args));
        }

        public void Play(AudioClipResolver audioClipResolver)
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(audioClipResolver);
        }
    }
}
