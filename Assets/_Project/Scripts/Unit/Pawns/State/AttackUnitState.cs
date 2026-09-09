using System.Collections;
using System.Collections.Generic;
using TheWarOfAddicts.Unit.UnitManage;
using TheWarOfAddicts.Utility;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class AttackUnitState: IUnitState
    {
        private static readonly WaitForSeconds AnimDelay = new WaitForSeconds(0.25f);
        
        
        private Pawn pawn;
        private Coroutine coroutine = null;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void OnEnter()
        {
            pawn.SetAnimatorState(PawnAnimStateType.Idle);
            coroutine = pawn.StartCoroutine(AttackCo());
        }
        public void OnUpdate()
        {
            // 적이 없는 경우 적을 탐색하는 과정
            if (!pawn.HasTarget)
            {
                Unit nearestTarget = UnitManager.Instance.FindNearestEnemy(pawn);

                if (!nearestTarget)
                {
                    pawn.Target = null;
                    pawn.ChangeState(UnitStateType.Idle);
                    return;
                }
                pawn.Target = nearestTarget;
            } 
            
            // 목표와의 거리가 너무 가깝거나, 너무 먼 지 파악하는 과정.
            float targetDist = pawn.transform.WidthDistance(pawn.Target.transform);
            if (targetDist < pawn.AttackMinRange)
            {
                pawn.ChangeState(UnitStateType.BackStep);
            }
            else if (targetDist > pawn.AttackMaxRange)
            {
                pawn.ChangeState(UnitStateType.Move);
            }
        }
        public void OnExit()
        {
            if (coroutine is not null)
            {
                pawn.StopCoroutine(coroutine);
            }
            // 해당 유닛이 공격 중에 타겟이 이동해도 공격을 받아야 함.
            //pawn.Target = null;
        }


        private IEnumerator AttackCo()
        {
            while (true)
            {
                float delay = pawn.AttackCooldownTime;
                if (delay > 0)
                {
                    yield return new WaitForSeconds(delay);
                }

                if (pawn.HasSkill && Random.Range(0, 1f) < pawn.SkillRate)
                {
                    pawn.ChangeState(UnitStateType.Skill);
                    yield break;
                }

                pawn.SetAttackCooldown();
                pawn.SetAnimatorState(pawn.Data.attackAnimation);
                yield return AnimDelay;
                pawn.SetAnimatorState(PawnAnimStateType.Idle);
            }
        }
    }
}