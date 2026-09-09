using Lean.Pool;
using TheWarOfAddicts.Projectile;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.Skill
{
    public class MultipleDamageRangeSkill: SkillBase
    {
        [SerializeField] private Transform throwPoint;
        [SerializeField] private ProjectileBase projectilePrefab;
        
        [SerializeField] private float rate = 1.5f;
        
        
        public override void OnUse(Unit target)
        {
            if (!target) return;
            
            ProjectileBase p = LeanPool.Spawn(projectilePrefab, throwPoint.position, Quaternion.identity);
            p.Init(target.transform.position, Mathf.RoundToInt(pawn.Damage * rate), pawn.ImpaleCount, pawn.ProjectileSpeed, pawn.Owner);
        }
    }
}