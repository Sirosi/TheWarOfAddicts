namespace TheWarOfAddicts.Unit.Pawns.State
{
    public interface IUnitState
    {
        public void Init(Pawn pawn);
        
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }
}