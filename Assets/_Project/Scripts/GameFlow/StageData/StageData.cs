using TheWarOfAddicts.Race;
using TheWarOfAddicts.Unit;
using UnityEngine;

namespace TheWarOfAddicts.GameFlow.StageData
{
    [CreateAssetMenu(menuName = "Stage/New Stage Data", fileName = "New Stage Data")]
    public class StageData: ScriptableObject
    {
        [Header("스테이지 기본 정보")]
        [Tooltip("스테이지 명칭")] public string nameCode;
        [Tooltip("전투 종류")] public BattleType battleType;
        [Tooltip("적 종족 데이터")] public RaceData enemyData;
        [Tooltip("덱 정보, 스모키드를 위한 옵션")] public UnitData[] playerDeck;
        
        [Header("배경 정보")]
        [Tooltip("길 종류")] public RoadType roadType;
        [Tooltip("배경 종류")] public BackgroundType backgroundType;
        
        [Header("전장 정보")]
        [Tooltip("전장 가로 사이즈")] public float mapWidth = 30f;
        [Tooltip("적 보스 데이터")] public UnitData enemyBossData;
        [Tooltip("적 생성 정보")] public SpawnData[] spawners;

        [Header("보상 정보")]
        [Tooltip("최초 공략 보상")] public RewardData onceReward;
        [Tooltip("기본 공략 보상")] public RewardData defaultReward;
    }
}