using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.GameFlow.StageData;
using TheWarOfAddicts.Race.Background;
using TheWarOfAddicts.Unit.Construction;
using TheWarOfAddicts.Unit.Player;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.GameFlow.InGame
{
    public class InGameManager: Singleton<InGameManager>, IBattleResultLockable
    {
        public const float MAX_AMOUNT = 0.5f;
        public const float MIN_AMOUNT = -0.5f;
        
        private const float CHECK_CYCLE = 0.5f;
        
        
        [SerializeField] private Transform spawnerPivot;
        [SerializeField] private CinemachineVirtualCamera cinemaCamera;
        [SerializeField] private Transform backgroundGroup;

        
        public PlayerData PlayerData => PlayerData.Instance;
        public StageData.StageData StageData { get; private set; }
        public PlayerBase Player { get; private set; }

        public Unit.Unit LeftestEnemy { get; private set; } = null;
        public Unit.Unit RightestAlly { get; private set; } = null;

        public float CenterAmount { get; private set; } = MIN_AMOUNT;

        
        protected override void Awake()
        {
            base.Awake();
            
            // Data를 내부 객체로 변경
            StageData = SessionData.SelectedStageData;

            // 스포너 위치 벌리기
            spawnerPivot.localScale = Vector3.one * StageData.mapWidth;
            
            // 스포너 추가
            foreach (SpawnData data in StageData.spawners)
            {
                UnitManager.Instance.AddSpawner(data);
            }

            // Cost, Mana Recharger
            StartCoroutine(FindLeftestEnemyCo());
            StartCoroutine(FindRightestAllyCo());
        }
        void Start()
        {
            PlayerResourceManager.Instance.Init();
            BackgroundDirector.Instance.Init(PlayerData);
            
            SettingMap(StageData);

            // 플레이어 생성
            if (PlayerData.RaceData.playerUnit)
            {
                Vector3 playerPos = spawnerPivot.position + Vector3.left * StageData.mapWidth * MAX_AMOUNT;
                
                Player = (PlayerBase)Unit.Unit.Spawn(PlayerData.RaceData.playerUnit, OwnerType.AllyGroup, playerPos);
                cinemaCamera.Follow = Player.transform; 
            }

            // 적 보스 or 적 기지 생성
            if (StageData.enemyBossData)
            {
                Vector3 enemyBossPos = spawnerPivot.position + Vector3.right * StageData.mapWidth * MAX_AMOUNT;
                
                Unit.Unit.Spawn(StageData.enemyBossData, OwnerType.EnemyGroup, enemyBossPos);
            }
        }

        void Update()
        {
            CalcCenterAmount();
        }


        public void Lock()
        {
            StopAllCoroutines();
        }


        private void SettingMap(StageData.StageData stageData)
        {
            //roadRenderer.sprite = null;
            backgroundGroup.position = (stageData.mapWidth * 0.5f + 0.5f) * Vector3.left;
            foreach (Transform child in backgroundGroup)
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                spriteRenderer.size = new Vector2(stageData.mapWidth + 1f, spriteRenderer.size.y);
            }
        }

        private void CalcCenterAmount()
        {
            Unit.Unit rightestUnit = RightestAlly;
            Unit.Unit leftestUnit = LeftestEnemy;

            if (rightestUnit || leftestUnit)
            {
                float posRight = rightestUnit ? rightestUnit.transform.position.x / StageData.mapWidth : MIN_AMOUNT;
                float posLeft = leftestUnit ? leftestUnit.transform.position.x / StageData.mapWidth : MAX_AMOUNT;


                if (posLeft < CenterAmount)
                {
                    CenterAmount = posLeft;
                }
                else if (posRight > CenterAmount)
                {
                    CenterAmount = posRight;
                }
            }
        }
        

        private IEnumerator FindLeftestEnemyCo()
        {
            while (true)
            {
                // 최좌측 적 유닛 탐색
                Unit.Unit leftestUnit = null;
                float leftestPosX = float.MaxValue;
                List<Unit.Unit> enemies = UnitManager.Instance.GetAllyUnits(OwnerType.EnemyGroup);

                foreach (var unit in enemies)
                {
                    if(unit is Construction) continue;
                    
                    float nowX = unit.transform.position.x;
                    if (nowX < leftestPosX)
                    {
                        leftestUnit = unit;
                        leftestPosX = nowX;
                    }
                }
                LeftestEnemy = leftestUnit;
                 
                // 가장 좌측에 있는 적을 기준으로 패배 판정.
                if (leftestPosX < UnitManager.Instance.AllySpawnPoint.position.x)
                {
                    BattleResultManager.Instance.ProcessDefeat();
                }

                yield return new WaitForSeconds(CHECK_CYCLE);
            }
        }

        private IEnumerator FindRightestAllyCo()
        {
            while (true)
            {
                // 최우측 아군 유닛 탐색
                Unit.Unit rightestUnit = null;
                float rightestPosX = float.MinValue;
                List<Unit.Unit> allys = UnitManager.Instance.GetAllyUnits(OwnerType.AllyGroup);

                foreach (var unit in allys)
                {
                    if(unit is Construction) continue;
                    
                    float nowX = unit.transform.position.x;
                    if (nowX > rightestPosX)
                    {
                        rightestUnit = unit;
                        rightestPosX = nowX;
                    }
                }
                RightestAlly = rightestUnit;

                yield return new WaitForSeconds(CHECK_CYCLE);
            }
        }
    }
}