using System.Collections.Generic;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class MoveUnitState: IUnitState
    {
        private Pawn pawn;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void OnEnter()
        {
            pawn.SetAnimatorState(pawn.Data.moveAnimation);
        }
        
        public void OnUpdate()
        {
            Unit nearestTarget = UnitManager.Instance.FindNearestEnemy(pawn, out float distance);
            if (nearestTarget is null) // 적이 없으면 Idle로 전환함.
            {
                pawn.ChangeState(UnitStateType.Idle);
                return;
            }
            
            
            if (distance <= pawn.AttackMaxRange - 0.25f) // 가장 가까운 적이 사거리 내에 있는지 확인
            {
                if (distance < pawn.AttackMinRange) // 최소거리보다 가까이 다가온 적이 있으면 백스탭이 필요함. 
                {
                    pawn.ChangeState(UnitStateType.BackStep);
                    return;
                }
                
                pawn.Target = nearestTarget;
                pawn.ChangeState(UnitStateType.Attack);
                return;
            }
            
            // 이동처리
            Vector2 dir = pawn.Owner == OwnerType.AllyGroup ? Vector2.right : Vector2.left;
            pawn.transform.Translate(pawn.MoveSpeed * Time.deltaTime * dir, Space.World);
        }
        
        public void OnExit()
        {
            
        }
    }
}