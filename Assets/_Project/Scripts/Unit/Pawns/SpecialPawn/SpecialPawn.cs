using System.Collections.Generic;
using Lean.Pool;
using TheWarOfAddicts.UI.InGame;
using TheWarOfAddicts.UI.InGame.Map;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.SpecialPawn
{
    public abstract class SpecialPawn: Unit
    {
        protected static readonly Dictionary<OwnerType, Color> OwnerColor = new()
        {
            { OwnerType.AllyGroup, Color.black },
            { OwnerType.EnemyGroup, new Color(0.1f, 0.1f, 0.1f, 1f) }
        };


        void Update()
        {
            if (IsLocked) return;
            
            OnUpdate();
        }
        
        
        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;

            base.Init(data, ownerType);
            
            HpBar = HpBarManager.Instance?.GetHpBar(this);
            HpBar?.Refresh(CurrentHp, MaxHp);

            MapIcon = MapManager.Instance?.SetMapIcon(transform, OwnerColor[Owner], 1.25f);
        }

        
        protected override void OnDie()
        {
            if (TryRelease())
            {
                LeanPool.Despawn(this);
            }
        }

        public override void Attack(Unit target)
        {
            // 할 역할이 없음.
        }

        
        /// <summary>
        /// 매 프레임마다 동작
        /// </summary>
        protected abstract void OnUpdate();
    }
}