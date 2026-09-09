namespace TheWarOfAddicts.GameFlow.InGame
{
    /// <summary>
    /// BattleResultManager에서 일시에 Lock을 걸어버리기 위한, Manager 구현용
    /// </summary>
    public interface IBattleResultLockable
    {
        public void Lock();
    }
}