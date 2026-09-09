using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.Skill
{
    public class MultipleDamageSkill: SkillBase
    {
        [SerializeField] private float rate = 1.5f;
        
        
        public override void OnUse(Unit target)
        {
            if(!pawn.HasTarget) return;

            target.TakeDamage(Mathf.RoundToInt(pawn.Damage * rate));
        }
    }
}