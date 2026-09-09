using TheWarOfAddicts.Unit.Pawns;
using UnityEngine;

namespace TheWarOfAddicts.Unit
{
    [CreateAssetMenu(menuName = "Unit/New Unit Data", fileName = "New Unit Data")]
    public class UnitData: ScriptableObject
    {
        [Header("오브젝트 정보")]
        [Tooltip("유닛 명칭")] public string nameCode;
        [Tooltip("캐릭터 오브젝트")] public Unit unitPrefab;
        
        [Space(50f)]
        [Header("기본 데이터")]
        public UnitBaseData unitBaseData;
        
        [Space(50f)]
        [Header("업그레이드 당 추가 데이터")]
        public UnitBaseData perUpgradeData;
        
        [Space(50f)]
        [Header("UI 정보")]
        [Tooltip("HP바 가로 사이즈")] public float hpBarSize = 1f;
        [Tooltip("HP바 오프셋")] public Vector2 hpBarOffset = new Vector2(0, 2.25f);
        
        [Space(20f)]
        [Header("스폰 정보")]
        [Tooltip("스폰 아이콘")] public Sprite icon;
        [Tooltip("기본 쿨타임")] public float cooldown = 1f;
        [Tooltip("기본 코스트")] public int cost = 5;
        [Tooltip("기본 코스트")] public Vector3 spawnOffset = Vector3.zero;
        
        [Header("애니메이션 정보")]
        [Tooltip("이동 애니메이션")] public PawnAnimStateType moveAnimation = PawnAnimStateType.Move;
        [Tooltip("공격 애니메이션")] public PawnAnimStateType attackAnimation = PawnAnimStateType.OnArmSmash;
        [Tooltip("스킬 애니메이션")] public PawnAnimStateType skillAnimation = PawnAnimStateType.TwoArmSmash;
        
    }
}