using System.Collections.Generic;
using Lean.Pool;
using TheWarOfAddicts.UI.InGame;
using TheWarOfAddicts.UI.InGame.Map;
using TheWarOfAddicts.Unit.Pawns.Skill;
using TheWarOfAddicts.Unit.Pawns.State;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns
{
    public class Pawn: Unit
    {
        private static readonly Dictionary<OwnerType, Color> OwnerColor = new()
        {
            { OwnerType.AllyGroup, Color.blue },
            { OwnerType.EnemyGroup, Color.red }
        };
        
        
        /// <summary>
        /// 공격하려고 잡아놓은 대상
        /// </summary>
        public Unit Target { get; set; } = null;
        /// <summary>
        /// 적합한 공격 대상이 존재하는지
        /// </summary>
        public bool HasTarget => Target && Target.gameObject.activeSelf && Target.IsAlive;
        /// <summary>
        /// 공격이 가능할 때까지 남은 시간
        /// </summary>
        public float AttackCooldownTime => AttackDelay + lastAttack - Time.time;

        public bool HasSkill => skill;
        

        private Dictionary<UnitStateType, IUnitState> States { get; } = new()
        {
            {UnitStateType.Idle, new IdleUnitState()},
            {UnitStateType.Stay, new StayUnitState()},
            {UnitStateType.Move, new MoveUnitState()},
            {UnitStateType.BackStep, new BackStepUnitState()},
            {UnitStateType.Attack, new AttackUnitState()},
            {UnitStateType.Skill, new SkillUnitState()},
            {UnitStateType.Die, new DieUnitState()},
        };
        
        
        private UnitStateType nowStateType = UnitStateType.Move;
        private IUnitState nowState = null;

        private SkillBase skill = null;
        private float lastAttack = 0f;


        protected override void Awake()
        {
            base.Awake();
            
            // 해당 Pawn의 스킬 참조
            skill = GetComponent<SkillBase>();
            skill?.Init(this);
            
            // StatePattern 초기화 작업.
            foreach (var state in States.Values)
            {
                state.Init(this);
            }
        }

        void Update()
        {
            if (IsLocked) return;
            
            nowState?.OnUpdate();
        }
        
        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;

            base.Init(data, ownerType);
            
            HpBar = HpBarManager.Instance?.GetHpBar(this);
            HpBar?.Refresh(CurrentHp, MaxHp);

            MapIcon = MapManager.Instance?.SetMapIcon(transform, OwnerColor[Owner]);
            
            ChangeState(UnitStateType.Idle);
        }
        
        
        public void ChangeState(UnitStateType newStateType)
        {
            nowStateType = newStateType;
            
            nowState?.OnExit();
            nowState = States[nowStateType];
            nowState.OnEnter();
        }
        
        public void SetAnimatorState(PawnAnimStateType state) => Animator.SetInteger("State", (int)state);
        public void SetAttackCooldown() => lastAttack = Time.time;
        
        public override void Attack(Unit target)
        {
            if(!HasTarget) return;

            target.TakeDamage(Damage);
        }
        private void Skill(Unit target)
        {
            skill?.OnUse(target);
            
            ChangeState(UnitStateType.Idle);
        }

        
        protected override void OnDie()
        {
            ChangeState(UnitStateType.Die);
        }
        /// <summary>
        /// Animation Event를 받았을 때 처리하는 부분
        /// </summary>
        public virtual void OnAnimDie()
        {
            LeanPool.Despawn(this);
        }


        public override void OnAnimAttack()
        {
            if (HasSkill && nowStateType == UnitStateType.Skill)
            {
                Skill(Target);
            }
            else
            {
                Attack(Target);
            }
        }
    }
}