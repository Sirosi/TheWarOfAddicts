using System.Collections;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class SkillUnitState: IUnitState
    {
        private Pawn pawn;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void OnEnter()
        {
            pawn.SetAttackCooldown();
            pawn.SetAnimatorState(pawn.Data.skillAnimation);
        }
        public void OnUpdate()
        {
            
        }
        public void OnExit()
        {
            
        }
    }
}