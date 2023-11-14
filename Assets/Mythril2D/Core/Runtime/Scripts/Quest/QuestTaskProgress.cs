using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public abstract class QuestTaskProgress
    {
        public QuestTask task = null;
        private Action<QuestTaskProgress> m_completionCallback;

        public QuestTaskProgress(QuestTask task)
        {
            this.task = task;
        }

        ~QuestTaskProgress()
        {
            OnProgressTrackingStopped();
        }

        public void Initialize(Action<QuestTaskProgress> completionCallback)
        {
            m_completionCallback = completionCallback;
            OnProgressTrackingStarted();
        }

        public abstract void OnProgressTrackingStarted();
        public abstract void OnProgressTrackingStopped();
        public abstract bool IsCompleted();

        public void UpdateProgression()
        {
            if (IsCompleted())
            {
                m_completionCallback(this);
                OnProgressTrackingStopped();
            }
        }
    }
}