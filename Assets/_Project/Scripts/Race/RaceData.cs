using TheWarOfAddicts.Race.Background;
using TheWarOfAddicts.Unit;
using TheWarOfAddicts.Unit.Player.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheWarOfAddicts.Race
{
    [CreateAssetMenu(menuName = "Race/New Race Data", fileName = "New Race Data")]
    public class RaceData: ScriptableObject
    {
        [Header("세력 기본 정도")]
        [Tooltip("세력 타입")] public RaceType raceType;
        [Tooltip("세력 배경 설정")] public BackgroundData backgroundData;
        
        
        [Header("플레이어 설정 값")]
        [Tooltip("플레이어블 캐릭터")] public UnitData playerUnit;
        [Tooltip("캐릭터 강화 가능 여부")] public bool canUpgradePlayer;
        
        [Header("세력 제약 사항")]
        [Tooltip("유닛 제한수 존재 여부")] public bool hasUnitLimit;
        [Tooltip("덱 구성 가능 여부")] public bool canUseDeck;
        [Tooltip("사용 가능한 유닛")] public UnitData[] raceUnits;
        [Tooltip("사용 가능한 유닛")] public WeaponData[] raceWeapons;
        
        [Header("초기 설정값")]
        [Tooltip("게임을 새로시작할 때 들어가는 값")] public InitialData initialData;


        public bool CanUseWeapon(string weaponCode)
        {
            foreach (WeaponData weapon in raceWeapons)
            {
                if (weaponCode == weapon.nameCode)
                {
                    return true;
                }
            }

            return false;
        }
    }
}