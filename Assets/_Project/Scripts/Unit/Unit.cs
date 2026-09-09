using System;
using Lean.Pool;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.UI.InGame;
using TheWarOfAddicts.UI.InGame.Map;
using TheWarOfAddicts.Unit.UnitManage;
using Unity.VisualScripting;
using UnityEngine;

namespace TheWarOfAddicts.Unit
{
    public abstract class Unit : MonoBehaviour
    {
        public UnitData Data { get; protected set; }

        public int Level { get; protected set; } = 0;

        public int CurrentHp { get; protected set; } = 0;
        public int MaxHp { get; protected set; } = 1;

        public int Damage { get; protected set; } = 1;
        public int Armor { get; protected set; } = 0;
        public float AttackMinRange { get; protected set; } = 0f;
        public float AttackMaxRange { get; protected set; } = 1f;
        public float SkillRate { get; protected set; } = 0.1f;

        public int ImpaleCount { get; protected set; } = 0;
        public float ProjectileSpeed { get; protected set; } = 1f;
        
        public float MoveSpeed
        {
            get => moveSpeed;
            set
            {
                moveSpeed = value;
                Animator.SetFloat("MoveSpeed", moveSpeed);
            }
        }
        public float AttackSpeed
        {
            get => attackSpeed;
            set => attackSpeed = value;
        }

        public float AttackDelay => 1 / AttackSpeed;
        public Animator Animator { get; private set; } = null;
        public OwnerType Owner { get; set; } = OwnerType.AllyGroup;
        public HpBar HpBar { get; set; } = null;
        public MapUnitIcon MapIcon { get; set; } = null;
        public bool IsAlive { get; private set; } = false;
        public bool IsLocked { get; set; } = false;
        
        
        private Collider2D collide = null;

        private float moveSpeed = 1f;
        private float attackSpeed = 1f;


        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            collide = GetComponent<Collider2D>();
        }


        /// <summary>
        /// 해당 Unit을 Pool에서 가져오거나, 생성한 후 해야 하는 초기화 작업
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ownerType"></param>
        protected virtual void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;

            Owner = ownerType;
            Data = data;
            
            IsAlive = true;
            collide.enabled = true;

            // 스테이터스 세팅
            InitStatus(data.unitBaseData, data.perUpgradeData);
            
            UnitManager.Instance.AddList(this);

            transform.localScale = new Vector3(Owner == OwnerType.AllyGroup ? 1 : -1, 1, 1);
            Animator.SetTrigger("Reset");
        }

        protected virtual void InitStatus(UnitBaseData baseData, UnitBaseData upgradeData)
        {
            PlayerData pData = PlayerData.Instance;
            
            Level = Owner == OwnerType.AllyGroup ? PlayerData.Instance.GetUnitUpgradeLevel(Data.nameCode) : 0;
            
            MaxHp = (int)CalcStatus(baseData.maxHp * pData.UnitHpRate, upgradeData.maxHp, Level);
            Damage = (int)CalcStatus(baseData.damage * pData.UnitDamageRate, upgradeData.damage, Level);
            Armor = (int)CalcStatus(baseData.armor, upgradeData.armor, Level);
            AttackMinRange = CalcStatus(baseData.attackMinRange, upgradeData.attackMinRange, Level);
            AttackMaxRange = CalcStatus(baseData.attackMaxRange, upgradeData.attackMaxRange, Level);
            SkillRate = CalcStatus(baseData.skillRate, upgradeData.skillRate, Level);

            ImpaleCount = (int)CalcStatus(baseData.impaleCount, upgradeData.impaleCount, Level);
            ProjectileSpeed = CalcStatus(baseData.projectileSpeed, upgradeData.projectileSpeed, Level);
            
            MoveSpeed = CalcStatus(baseData.moveSpeed * pData.UnitMoveSpeedRate, upgradeData.moveSpeed, Level);
            AttackSpeed = CalcStatus(baseData.attackSpeed * pData.UnitAttackSpeedRate, upgradeData.attackSpeed, Level);
            
            CurrentHp = MaxHp;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 해당 Unit을 다시 Pool로 되돌리는 작업
        /// </summary>
        public bool TryRelease()
        {
            if (!IsAlive) return false;

            IsAlive = false;
            collide.enabled = false;
            
            UnitManager.Instance.RemoveList(this);
            if (HpBar)
            {
                LeanPool.Despawn(HpBar);
            }
            if (MapIcon)
            {
                LeanPool.Despawn(MapIcon);
            }

            return true;
        }
        public void ForceRelease()
        {
            if (TryRelease())
            {
                LeanPool.Despawn(this);
            }
        }

        public void TakeDamage(int damage, bool isAbsolute = false)
        {
            if(damage < 0) return; // 혹시나 하는 상황을 대비한 예외처리
            
            int realDamage = Math.Clamp(damage - (isAbsolute ? 0 : Armor), 0, damage);
            if (realDamage <= 0) return;
            
            CurrentHp -= Math.Clamp(realDamage, 0, damage);
            OnDamaged(damage, realDamage);
            
            if (CurrentHp <= 0)
            {
                OnDie();
            }
        }


        /// <summary>
        /// Animation Event를 받았을 때 처리하는 부분
        /// </summary>
        public virtual void OnAnimAttack()
        {
            // 기본적으로 하는 게 없음.
        }
        

        /// <summary>
        /// 대상을 공격할 때의 행동
        /// </summary>
        /// <param name="target"></param>
        public abstract void Attack(Unit target);
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 체력을 전부 소진했을 때의 행동
        /// </summary>
        protected abstract void OnDie();

        /// <summary>
        /// 데미지를 입었을 때 행동
        /// </summary>
        protected virtual void OnDamaged(int damage, int realDamage)
        {
            HpBar?.Refresh(CurrentHp, MaxHp);
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public static Unit Spawn(UnitData data, OwnerType ownerType, Vector3 pos, Transform parent = null)
        {
            Unit unit = LeanPool.Spawn(data.unitPrefab, pos + data.spawnOffset, Quaternion.identity, parent);
            unit.Init(data, ownerType);
            
            return unit;
        }

        protected static float CalcStatus(float defValue, float perValue, int unitLevel)
        {
            return defValue + perValue * unitLevel;
        }
    }
}