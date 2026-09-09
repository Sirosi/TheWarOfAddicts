using TheWarOfAddicts.GameFlow.InGame;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Construction
{
    public class Barracks: Construction
    {
        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;

            base.Init(data, ownerType);
            
            transform.localScale = Vector3.one;
        }
        
        public override void Attack(Unit target)
        {
            // 행동 없음.
            // 배럭은 공격을 하는 존재가 아님.
        }
        protected override void OnDie()
        {
            BattleResultManager.Instance.ProcessVictory();
        }
        protected override void OnDamaged(int damage, int realDamage)
        {
            base.OnDamaged(damage, realDamage);
            
            // TODO: UI 체력바 감소 등의 기능.
        }
    }
}