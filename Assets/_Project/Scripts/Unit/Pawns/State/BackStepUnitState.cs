using System.Collections.Generic;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class BackStepUnitState: IUnitState
    {
        private Pawn pawn;
        private float preMoveSpeed = 1f;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void OnEnter()
        {
            preMoveSpeed = pawn.MoveSpeed;
            pawn.MoveSpeed *= 0.5f;
            
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
            
            
            if (distance >= pawn.AttackMinRange + 0.25f) // 가장 가까운 적이 사거리 내에 있는지 확인
            {
                if (distance > pawn.AttackMaxRange) // 최대거리보다 먼 적이 있으면 다가가는 게 필요함. 
                {
                    pawn.ChangeState(UnitStateType.Move);
                    return;
                }
                
                pawn.Target = nearestTarget;
                pawn.ChangeState(UnitStateType.Attack);
                return;
            }
            
            Vector2 dir = pawn.Owner == OwnerType.AllyGroup ? Vector2.left : Vector2.right;
            pawn.transform.Translate(pawn.MoveSpeed * Time.deltaTime * dir, Space.World);
        }
        public void OnExit()
        {
            pawn.MoveSpeed = preMoveSpeed;
        }
    }
}