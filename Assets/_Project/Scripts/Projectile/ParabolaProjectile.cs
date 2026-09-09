using UnityEngine;

namespace TheWarOfAddicts.Projectile
{
    public class ParabolaProjectile: ProjectileBase
    {
        [SerializeField] private float minHeight = 0.5f;
        [SerializeField] private float maxHeight = 3f;
        
        
        private float height = 1;
        
        private float throwingTime = 0;
        
        private Vector2 startPos = Vector2.zero;
        private Vector2 direction = Vector2.zero;
        
        
        protected override void OnInit()
        {
            direction = Target - (Vector2)transform.position;
            //if (direction == Vector2.zero) ; // 포물선 공격은 신경쓸 필요가 없음.

            startPos = transform.position;
            
            float widthDist = Mathf.Abs(Target.x - transform.position.x);
            height = Mathf.Clamp(widthDist * 0.25f, minHeight, maxHeight);
            throwingTime = 0; // 주행 시간이므로 OnInit에서 초기화해 줘야함.
        }
        
        protected override void Move()
        {
            throwingTime += Time.deltaTime;
            float t = throwingTime / ThrowingDuration;

            Vector2 pos = startPos + direction * t;
            pos.y += Mathf.Sin(t * Mathf.PI) * height;
            
            transform.position = pos;
        }
    }
}