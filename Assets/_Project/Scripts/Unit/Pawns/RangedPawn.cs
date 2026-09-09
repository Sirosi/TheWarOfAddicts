using Lean.Pool;
using TheWarOfAddicts.Projectile;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns
{
    public class RangedPawn: Pawn
    {
        [SerializeField] private Transform throwPoint;
        [SerializeField] private ProjectileBase projectilePrefab;
        
        
        public override void Attack(Unit target)
        {
            if (!target) return;
            
            ProjectileBase p = LeanPool.Spawn(projectilePrefab, throwPoint.position, Quaternion.identity);
            p.Init(target.transform.position, Damage, ImpaleCount, ProjectileSpeed, Owner);
        }
    }
}