using System.Collections;
using Lean.Pool;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Projectile
{
    // 충돌 및 모든 사항은 Base에서 작성하고, 해당 Base를 상속받은 Projectile은 오로지 이동처리만 담당하게 설계.
    public abstract class ProjectileBase: MonoBehaviour
    {
        /// <summary>
        /// 목표 지점
        /// </summary>
        protected Vector2 Target { get; private set; }
        /// <summary>
        /// 투사체 속도
        /// </summary>
        protected float MoveSpeed { get; private set; }
        /// <summary>
        /// 예상 주행 시간
        /// </summary>
        protected float ThrowingDuration { get; private set; }
        /// <summary>
        /// 해당 피사체의 피아식별 타입
        /// </summary>
        protected OwnerType OwnerType { get; private set; } = OwnerType.AllyGroup;

        
        private int damage = 0;
        private int impaleCount = 0;

        private bool isUsed = false;


        void Update()
        {
            Move();
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (isUsed) return;
            
            if (other.TryGetComponent(out Unit.Unit unit) && unit.Owner != OwnerType)
            {
                unit.TakeDamage(damage);

                if (impaleCount <= -1) return;
                
                if (--impaleCount < 0)
                {
                    Release();
                }
            }
        }
        
        
        /// <summary>
        /// Pool에서 끌고왔을 때 사용을 위해 호출
        /// </summary>
        /// <param name="target">공격 대상</param>
        /// <param name="damage">피해량</param>
        /// <param name="moveSpeed">피사체 속도</param>
        /// <param name="owner">피사체 주인 타입</param>
        public void Init(Vector2 target, int damage, int impaleCount, float moveSpeed, OwnerType ownerType)
        {
            this.Target = target;
            this.damage = damage;
            this.impaleCount = impaleCount;
            this.MoveSpeed = moveSpeed;
            this.OwnerType = ownerType;

            isUsed = false;
            
            float distance = Vector2.Distance(transform.position, target);
            ThrowingDuration = distance / moveSpeed;
            SetLifeTimer(ThrowingDuration);
            
            OnInit();
        }

        /// <summary>
        /// Pool로 돌려보내거나, 삭제하는 종료 기능
        /// </summary>
        public void Release()
        {
            isUsed = true;
            LeanPool.Despawn(this);
        }

        protected void SetLifeTimer(float time) => StartCoroutine(LifeTimerCo(time));

        private IEnumerator LifeTimerCo(float time)
        {
            yield return new WaitForSeconds(time);
            
            Release();
        }

        
        /// <summary>
        /// Init이 된 이후 호출됨.
        /// </summary>
        protected abstract void OnInit();
        /// <summary>
        /// 매 Frame마다 호출됨.
        /// </summary>
        protected abstract void Move();
    }
}