using TheWarOfAddicts.Unit.UnitManage;

namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class IdleUnitState: IUnitState
    {
        private Pawn pawn;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void OnEnter()
        {
            pawn.SetAnimatorState(PawnAnimStateType.Idle);
        }
        public void OnUpdate()
        {
            if (UnitManager.Instance.GetEnemyUnits(pawn.Owner).Count > 0)
            {
                pawn.ChangeState(UnitStateType.Move);
            }
        }
        public void OnExit()
        {
            
        }
    }
}