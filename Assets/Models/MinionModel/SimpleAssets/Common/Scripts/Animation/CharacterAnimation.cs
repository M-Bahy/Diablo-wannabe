using System;
using System.Collections.Generic;
using System.Linq;
using SimpleAssets.Common.Animation;
using SimpleAssets.Common.Utils;
using UnityEngine;

namespace SimpleAssets.Common.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour
    {
        private static readonly int MoveSpeedHash = Animator.StringToHash(AnimationID.WALK_SPEED);
        private static readonly int RunSpeedHash = Animator.StringToHash(AnimationID.RUN_SPEED);
        private static readonly int AttackSpeedHash = Animator.StringToHash(AnimationID.ATTACK_SPEED);
        
        private static readonly int WalkHash = Animator.StringToHash(AnimationID.WALK);
        private static readonly int RunHash = Animator.StringToHash(AnimationID.RUN);
        private static readonly int IdleHash = Animator.StringToHash(AnimationID.IDLE);
        private static readonly int AttackHash = Animator.StringToHash(AnimationID.ATTACK);

        public event Action OnAttackReachEnd;
        public event Action OnAttackExit;
        public event Action OnDamageTrigger;

        private Animator Animator { get; set; }
        private Dictionary<string, AnimationStateMachine> States { get; set; }

        private void Awake() => 
            Initialize();

        private void OnDestroy()
        {
            if (States == null)
                return;
            if (States.TryGetValue(AnimationID.ATTACK, out var state))
            {
                state.OnExit -= AttackAnimationExit;
                state.OnReachEnd -= AttackAnimationReachEnd;
            }
        }

        private void Initialize()
        {
            Animator = GetComponent<Animator>();
            var stateMachineBehaviours = Animator.GetBehaviours<AnimationStateMachine>();
            States = stateMachineBehaviours.ToDictionary(x => x.StateName, x => x);
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            if (States.TryGetValue(AnimationID.ATTACK, out var state))
            {
                state.OnExit += AttackAnimationExit;
                state.OnReachEnd += AttackAnimationReachEnd;
            }
        }

        // play behavior
        public void PlayWalk()
        {
            Animator.SetTrigger(WalkHash);
        }
        
        public void PlayRun()
        {
            Animator.SetTrigger(RunHash);
        }
        
        public void PlayIdle()
        {
            Animator.SetTrigger(IdleHash);
        }

        public void PlayAttack()
        {
            /*if (gameObject.name == "Ghoul-Blue (Action RPG Player)(Clone)")
                Debug.Log("PlayAttack");*/
            Animator.SetTrigger(AttackHash);
        }

        // speed beahavior
        public void SetWalkSpeed(float speed)
        {
            Animator.SetFloat(MoveSpeedHash, speed);
        }
        
        public void SetRunSpeed(float speed)
        {
            Animator.SetFloat(RunSpeedHash, speed);
        }
        
        public void SetAttackSpeed(float speed)
        {
            Animator.SetFloat(AttackSpeedHash, speed);
        }

        // reset behavoir
        public void ResetIdleTrigger()
        {
            Animator.ResetTrigger(IdleHash);
        }
        
        public void ResetAttackTrigger()
        {
            Animator.ResetTrigger(AttackHash);
        }
        
        public void ResetRunTrigger()
        {
            Animator.ResetTrigger(RunSpeedHash);
        }
        
        public void ResetWalkTrigger()
        {
            Animator.ResetTrigger(WalkHash);
        }
        
        // animation events
        private void AttackAnimationExit()
        {
            OnAttackExit?.Invoke();
        }
        
        private void AttackAnimationReachEnd()
        {
            OnAttackReachEnd?.Invoke();
        }
        
        private void DamageTrigger()
        {
            OnDamageTrigger?.Invoke();
        }
    }
}