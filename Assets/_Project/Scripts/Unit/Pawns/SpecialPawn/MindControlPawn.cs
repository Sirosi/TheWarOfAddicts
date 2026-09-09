using Lean.Pool;
using TheWarOfAddicts.Unit.Construction;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.SpecialPawn
{
    public class MindControlPawn: SpecialPawn
    {
        private bool isUsed = false;
        
        
        private void OnTriggerEnter2D(Collider2D other)
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

                UnitManager.Instance.Units[unit.Owner].Remove(unit);
                UnitManager.Instance.Units[Owner].Add(unit);
                
                unit.Owner = Owner;
                unit.HpBar.Offset = new Vector2(-unit.HpBar.Offset.x, unit.HpBar.Offset.y);
                unit.transform.localScale = new Vector3(Owner == OwnerType.AllyGroup ? 1 : -1, 1, 1);
                unit.Animator.SetTrigger("Reset");
                
                OnDie();
            }
        }


        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;
            
            base.Init(data, ownerType);

            isUsed = false;
            Animator.SetInteger("State", (int)PawnAnimStateType.Move);
        }
        
        
        protected override void OnUpdate()
        {
            Vector2 dir = Owner == OwnerType.AllyGroup ? Vector2.right : Vector2.left;
            transform.Translate(MoveSpeed * Time.deltaTime * dir, Space.World);
        }
    }
}