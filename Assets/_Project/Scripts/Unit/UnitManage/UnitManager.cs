using System.Collections;
using System.Collections.Generic;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.GameFlow.InGame;
using TheWarOfAddicts.GameFlow.StageData;
using TheWarOfAddicts.Unit.Pawns;
using TheWarOfAddicts.Unit.Pawns.SpecialPawn;
using TheWarOfAddicts.Unit.Pawns.State;
using UnityEngine;

namespace TheWarOfAddicts.Unit.UnitManage
{
    public class UnitManager: Singleton<UnitManager>, IBattleResultLockable
    {
        [SerializeField] [Tooltip("유닛을 묶을 위치")] private Transform unitGroup;
        
        [SerializeField] [Tooltip("유닛 생성 오차범위")] private float createRadius = 1f;
        
        [Header("스폰 위치")]
        [SerializeField] private Transform allySpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;
        
        
        public Transform AllySpawnPoint => allySpawnPoint;
        public Transform EnemySpawnPoint => enemySpawnPoint;
        
        
        private Stack<Coroutine> coroutines = new(10);
        
        
        public readonly Dictionary<OwnerType, List<Unit>> Units = new()
        {
            {OwnerType.AllyGroup, new List<Unit>()},
            {OwnerType.EnemyGroup, new List<Unit>()},
        };
        public readonly List<WitchPawn> Witches = new();



        public void Lock()
        {
            foreach(IEnumerable units in Units.Values)
            {
                foreach (Unit unit in units)
                {
                    if (unit is Pawn pawn)
                    {
                        pawn.ChangeState(UnitStateType.Stay);
                    }
                    unit.IsLocked = true;
                }
            }
            foreach (WitchPawn witch in Witches)
            {
                witch.IsLocked = true;
            }
            ClearSpawners();
        }
        
        public void AddSpawner(SpawnData spawnData)
        {
            coroutines.Push(StartCoroutine(SpawnEnemyCo(spawnData)));
        }

        public void ClearSpawners()
        {
            while (coroutines.Count > 0)
            {
                StopCoroutine(coroutines.Pop());
            }
        }

        public void SpawnAllyPawn(UnitData data)
        {
            SpawnPawn(OwnerType.AllyGroup, AllySpawnPoint.position, data);
        }
        public void SpawnEnemyUnit(UnitData data)
        {
            SpawnPawn(OwnerType.EnemyGroup, EnemySpawnPoint.position, data);
        }

        public void AddList(Unit unit)
        {
            Units[unit.Owner]?.Add(unit);
        }
        public void RemoveList(Unit unit)
        {
            Units[unit.Owner]?.Remove(unit);
        }

        public List<Unit> GetAllyUnits(OwnerType ownerType)
        {
            return Units[ownerType];
        }
        public List<Unit> GetEnemyUnits(OwnerType ownerType)
        {
            return Units[ownerType == OwnerType.AllyGroup ? OwnerType.EnemyGroup : OwnerType.AllyGroup];
        }

        public Unit FindNearestEnemy(Unit unit) => FindNearestEnemy(unit, out _);
        public Unit FindNearestEnemy(Unit unit, out float distance)
        {
            List<Unit> enemies = GetEnemyUnits(unit.Owner);
            distance = float.MaxValue;
            if (enemies.Count <= 0) return null;
            
            // 가장 가까운 적의 위치 계산
            Unit nearestTarget = null;
            foreach (Unit enemy in enemies)
            {
                float dist = Mathf.Abs(enemy.transform.position.x - unit.transform.position.x);
                if (dist < distance)
                {
                    distance = dist;
                    nearestTarget = enemy;
                }
            }

            return nearestTarget;
        }
        
        
        private void SpawnPawn(OwnerType ownerType, Vector2 pos, UnitData data)
        {
            Vector2 offset = data.spawnOffset;
            if (offset == Vector2.zero)
            {
                offset = Random.insideUnitCircle * createRadius;
            }
            Unit.Spawn(data, ownerType, pos + offset, unitGroup);
        }

        private IEnumerator SpawnEnemyCo(SpawnData spawnData)
        {
            yield return new WaitForSeconds(spawnData.startCooldown);
            
            while (true)
            {
                SpawnEnemyUnit(spawnData.data);
                    
                float delay = spawnData.spawnInterval + Random.Range(0f, 1f);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}