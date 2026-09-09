using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.Skill
{
    [RequireComponent(typeof(Pawn))]
    public abstract class SkillBase: MonoBehaviour
    {
        protected Pawn pawn = null;
        
        
        public void Init(Pawn pawn)
        {
            this.pawn = pawn;
        }


        public abstract void OnUse(Unit target);
    }
}