using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Projectile
{
    public class LinearProjectile: ProjectileBase
    {
        private Vector2 moveDir = Vector2.zero;
        
        
        protected override void OnInit()
        {
            Vector2 direction = Target - (Vector2)transform.position;
            if (direction == Vector2.zero)
            {
                // 적과 위치가 일치하면, 좌/우로 발사함.
                direction = (OwnerType == OwnerType.AllyGroup ? 1 : -1) * Vector2.right;
            }
            
            moveDir = (direction + Vector2.up).normalized; // Vector2.up을 더하지 않으면, 발을 향해 공격하므로 y 1을 더해주는 거임.
        }
        
        protected override void Move()
        {
            transform.Translate(MoveSpeed * Time.deltaTime * moveDir);
        }
    }
}