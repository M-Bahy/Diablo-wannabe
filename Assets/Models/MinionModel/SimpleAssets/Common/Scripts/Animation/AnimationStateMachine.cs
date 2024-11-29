using System;
using UnityEngine;

namespace SimpleAssets.Common.Animation
{
    public class AnimationStateMachine : StateMachineBehaviour
    {
        public string StateName;
        public event Action OnExit;
        public event Action OnReachEnd;
        private bool ReachedEnd { get; set; }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ReachedEnd = false;
            OnExit?.Invoke();
            if (stateInfo.normalizedTime >= 0.99f)
            {
                OnReachEnd?.Invoke();
            }
        }
    }
}