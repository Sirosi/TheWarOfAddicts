using System.Collections;
using Lean.Pool;
using TheWarOfAddicts.Unit.Construction;
using TheWarOfAddicts.Unit.Pawns.State;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Pawns.SpecialPawn
{
    public class BumperPawn: SpecialPawn
    {
        [SerializeField] private UnitData spawnUnitData;
        [SerializeField] private Transform spawnPoint;
        
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private float spawnHpRate = 0.5f;


        private bool hasSpawned = false;
        private float maxMoveSpeed = 1f;
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Barracks barracks))
            {
                if (barracks.Owner == Owner) return;
                
                TakeDamage(CurrentHp, true);
            }
            else if (other.TryGetComponent(out Pawn pawn)) // 오로지 Pawn만 칠 수 있게 해야 함.
            {
                if (pawn.Owner == Owner) return;
                
                Animator.SetTrigger("Bump");
                
                pawn.TakeDamage(CurrentHp);
                int stockHp = pawn.CurrentHp;

                if (stockHp <= 0) // 적이 죽었을 때
                {
                    pawn.ChangeState(UnitStateType.Stay);
                    pawn.StartCoroutine(BumpingCo(pawn, MoveSpeed * 2));
                }

                int myDamage = CurrentHp + stockHp; // 치인 상대가 죽었다면, 체력이 음수가 될 것이기 때문에.
                this.TakeDamage(myDamage, true);
            }
        }


        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;

            base.Init(data, ownerType);

            maxMoveSpeed = MoveSpeed;
            MoveSpeed = 0;
            hasSpawned = false;

            if (spawnPoint)
            {
                spawnPoint.gameObject.SetActive(true);
            }
        }
        
        
        protected override void OnUpdate()
        {
            Vector2 dir = Owner == OwnerType.AllyGroup ? Vector2.right : Vector2.left;
            MoveSpeed += maxMoveSpeed / smoothTime * Time.deltaTime;
            MoveSpeed = MoveSpeed > maxMoveSpeed ? maxMoveSpeed : MoveSpeed;
            transform.Translate(MoveSpeed * Time.deltaTime * dir, Space.World);
        }

        protected override void OnDamaged(int damage, int realDamage)
        {
            base.OnDamaged(damage, realDamage);

            if (!spawnPoint) return;
            if (hasSpawned) return;
            
            if (CurrentHp <= Mathf.RoundToInt(MaxHp * spawnHpRate))
            {
                Unit.Spawn(spawnUnitData, Owner, spawnPoint.position, transform.parent);
                
                hasSpawned = true;
                spawnPoint.gameObject.SetActive(false);
            }
        }


        private static IEnumerator BumpingCo(Pawn pawn, float flyingStrength)
        {
            // 날아가는 Pawn의 날아갈 시간, 회전 방향, 날아가는 방향까지 계산 
            int ownerDir = pawn.Owner == OwnerType.AllyGroup ? -1 : 1;
            float startTime = Time.time;
            float flyingTime = 10 / flyingStrength;
            float rotateSpeed =  ownerDir * flyingStrength * 180;
            Vector2 flyingDir = new Vector2(ownerDir * flyingStrength, flyingStrength);
            
            // 실 계산을 실행하는 곳
            while (Time.time < startTime + flyingTime)
            {
                pawn.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                pawn.transform.Translate(flyingDir * Time.deltaTime, Space.World);
                yield return null;
            }
            
            // 죽이기
            pawn.TakeDamage(pawn.CurrentHp, true);
        }
    }
}