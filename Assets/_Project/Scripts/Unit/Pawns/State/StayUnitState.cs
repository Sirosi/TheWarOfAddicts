namespace TheWarOfAddicts.Unit.Pawns.State
{
    public class StayUnitState: IUnitState
    {
        private Pawn pawn = null;
        
        
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
            
        }
        public void OnExit()
        {
            
        }
    }
}