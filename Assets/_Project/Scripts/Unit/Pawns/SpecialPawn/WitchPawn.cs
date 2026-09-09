using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using TheWarOfAddicts.Unit.Construction;
using TheWarOfAddicts.Unit.Player;
using TheWarOfAddicts.Unit.UnitManage;
using TheWarOfAddicts.Utility;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.SpecialPawn
{
    public class WitchPawn: SpecialPawn
    {
        [SerializeField] private UnitData toChangeData;
        
        
        private bool isUsed = false;
        
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Barracks barracks))
            {
                if (barracks.Owner == Owner) return;
                
                TakeDamage(CurrentHp, true);
            }
            else if (other.TryGetComponent(out Unit unit)) // 오로지 Pawn만 칠 수 있게 해야 함.
            {
                if (unit.Owner == OwnerType.AllyGroup) return;
                if (isUsed) return;
                
                isUsed = true;
                
                // 피아식별을 가리지 않고, 유닛을 확인
                List<Unit> units = UnitManager.Instance.GetAllyUnits(Owner).ToList();
                units.AddRange(UnitManager.Instance.GetEnemyUnits(Owner));
                Stack<Unit> delStack = new();

                foreach (Unit u in units)
                {
                    // 플레이어와 건물을 제외한
                    if(u is PlayerBase) continue;
                    if(u is Construction.Construction) continue;
                    
                    // 공격범위 내의 모든 유닛을 toChangeData의 유닛으로 변경
                    float distance = u.transform.WidthDistance(transform);
                    if (distance <= AttackMaxRange)
                    {
                        delStack.Push(u);
                        
                        Vector2 pos = u.transform.position;
                        Unit.Spawn(toChangeData, u.Owner, pos, transform.parent);
                    }
                }

                // toChangeData의 유닛으로 변경된 유닛은 모두 강제 릴리즈
                while (delStack.Count > 0)
                {
                    delStack.Pop().ForceRelease();
                }
                
                // 작업이 끝나면 사망처리
                OnDie();
            }
        }


        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;
            
            base.Init(data, ownerType);
            
            UnitManager.Instance.RemoveList(this); // Witch는 공격을 받을 수 있는 대상이 아님.
            UnitManager.Instance.Witches.Add(this);

            isUsed = false;
        }
        
        
        protected override void OnUpdate()
        {
            Vector2 dir = Owner == OwnerType.AllyGroup ? Vector2.right : Vector2.left;
            transform.Translate(MoveSpeed * Time.deltaTime * dir, Space.World);
        }
        
        protected override void OnDie()
        {
            UnitManager.Instance.Witches.Remove(this);
            if (TryRelease())
            {
                LeanPool.Despawn(this);
            }
        }
    }
}