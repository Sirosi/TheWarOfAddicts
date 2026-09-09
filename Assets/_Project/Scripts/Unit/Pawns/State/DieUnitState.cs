using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class DieUnitState: IUnitState
    {
        private Pawn pawn;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void OnEnter()
        {
            pawn.TryRelease();
            pawn.Animator.SetTrigger("Die");
        }
        public void OnUpdate()
        {
            
        }
        public void OnExit()
        {
            
        }
    }
}