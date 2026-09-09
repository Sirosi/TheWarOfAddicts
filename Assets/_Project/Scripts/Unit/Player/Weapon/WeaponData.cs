using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.Projectile;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Player.Weapon
{
    [CreateAssetMenu(menuName = "Weapon/New Data", fileName = "New Weapon Data")]
    public class WeaponData: ScriptableObject
    {
        [Header("오브젝트 정보")]
        [Tooltip("무기 명칭")] public string nameCode;
        [Tooltip("무기 사용 시 생성될 프리팹")] public ProjectileBase prefab;
        [Tooltip("손에 쥐어질 이미지")] public Sprite handedSprite;
        [Tooltip("사용 애니메이션")] public PlayerAnimArmType animArmType = PlayerAnimArmType.None;
        
        [Space(50f)]
        [Header("기본 데이터")]
        public WeaponBaseData weaponBaseData;
        
        [Space(50f)]
        [Header("업그레이드 당 추가 데이터")]
        public WeaponBaseData perUpgradeData;
        
        [Header("UI 정보")]
        [Tooltip("아이콘")] public Sprite icon;


        public int UpgradeLevel => PlayerData.Instance?.GetWeaponUpgradeLevel(nameCode) ?? 0;
        
        public int Influence => weaponBaseData.influence + perUpgradeData.influence * UpgradeLevel;
        public float Cooldown => weaponBaseData.cooldown - perUpgradeData.cooldown * UpgradeLevel;
        public int Cost => weaponBaseData.cost - perUpgradeData.cost * UpgradeLevel;
        
        public float ProjectileSpeed => weaponBaseData.projectileSpeed + perUpgradeData.projectileSpeed * UpgradeLevel;
        public int ImpaleCount => weaponBaseData.impaleCount + perUpgradeData.impaleCount * UpgradeLevel;
    }
}