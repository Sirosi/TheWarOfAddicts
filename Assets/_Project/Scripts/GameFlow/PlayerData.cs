using System.Collections.Generic;
using Newtonsoft.Json;
using TheWarOfAddicts.Race;
using TheWarOfAddicts.Unit;
using TheWarOfAddicts.Unit.Player.Weapon;
using UnityEngine;

namespace TheWarOfAddicts.GameFlow
{
    [JsonObject(MemberSerialization.Fields)]
    public class PlayerData
    {
        // JSON으로 저장하기 위해선, Instance를 두는 편이 편함.
        public static PlayerData Instance { get; private set; } = new();


        private const string SAVE_PREFS = "SaveData";
        
        private const int DEFAULT_DECK_SIZE = 3;
        
        private const float DEFAULT_RECHARGE_SPEED = 1f;
        private const float PER_LEVEL_RECHARGE_SPEED = 0.25f;
        private const int DEFAULT_RESOURCE_SIZE = 20;
        private const int PER_LEVEL_RESOURCE_SIZE = 10;

        private const float PER_LEVEL_UNIT_DATA_RATE = 0.05f;
        private const float PER_LEVEL_PLAYER_DATA_RATE = 0.1f;
        
        
        public RaceData RaceData => SODataManager.Instance.GetRaceData(raceType);
        public UnitData[] Deck
        {
            get
            {
                Stack<UnitData> result = new(deck.Length);
                foreach (string unit in deck)
                {
                    result.Push(SODataManager.Instance.GetUnitData(unit));
                }

                return result.ToArray();
            }
            set
            {
                deck = new string[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    deck[i] = value[i].nameCode;
                }
            }
        }

        public WeaponData[] Weapons
        {
            get
            {
                // TODO: 시간관계상 Weapon의 Deck화는 어려울 것으로 보임.
                //  언젠가의 나는 그걸 해결할 것이라고 Believe함.
                Stack<WeaponData> result = new(weapons.Length);
                foreach (WeaponData weapon in RaceData.raceWeapons)
                {
                    if (HasWeapon(weapon.nameCode))
                    {
                        result.Push(weapon);
                    }
                }
                return result.ToArray();
                
                
                foreach (string weapon in weapons)
                {
                    result.Push(SODataManager.Instance.GetWeaponData(weapon));
                }

                return result.ToArray();
            }
            set
            {
                weapons = new string[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    weapons[i] = value[i].nameCode;
                }
            }
        }

        public int DeckSize => DeckLevel + DEFAULT_DECK_SIZE;
        
        public float CostRechargeDelay => DEFAULT_RECHARGE_SPEED - PER_LEVEL_RECHARGE_SPEED * CostRechargeLevel;
        public float ManaRechargeDelay => DEFAULT_RECHARGE_SPEED - PER_LEVEL_RECHARGE_SPEED * ManaRechargeLevel;
        public int MaxCost => DEFAULT_RESOURCE_SIZE + PER_LEVEL_RESOURCE_SIZE * MaxCostLevel;
        public int MaxMana => DEFAULT_RESOURCE_SIZE + PER_LEVEL_RESOURCE_SIZE * MaxManaLevel;

        public int StartCost => MaxCostLevel >= 2 ? 20 : 0;
        public int StartMana => MaxManaLevel >= 2 ? 20 : 0;
        
        public float PlayerHpRate => 1f + PER_LEVEL_PLAYER_DATA_RATE * PlayerHpLevel;
        public float PlayerMoveSpeedRate => 1f + PER_LEVEL_PLAYER_DATA_RATE * PlayerMoveSpeedLevel;
        
        public float UnitDamageRate => 1f + PER_LEVEL_UNIT_DATA_RATE * UnitDamageLevel;
        public float UnitAttackSpeedRate => 1f + PER_LEVEL_UNIT_DATA_RATE * UnitAttackSpeedLevel;
        public float UnitHpRate => 1f + PER_LEVEL_UNIT_DATA_RATE * UnitHpLevel;
        public float UnitMoveSpeedRate => 1f + PER_LEVEL_UNIT_DATA_RATE * UnitMoveSpeedLevel;


        #region ◇ 플레이어 정보 ◇
        private RaceType raceType = RaceType.Alcoho_Oland;
        
        private string[] deck = null;
        private string[] weapons = null;
        public int Money = 0;
        #endregion

        #region ◇ 플레이어 강화 정보 ◇
        public int PlayerLevel = 0;
        public int PlayerHpLevel = 0;
        public int PlayerMoveSpeedLevel = 0;
        #endregion

        #region ◇ 리소스 강화 정보 ◇
        public int CostRechargeLevel = 0;
        public int ManaRechargeLevel = 0;
        public int MaxCostLevel = 0;
        public int MaxManaLevel = 0;
        public int DeckLevel = 0;
        #endregion

        #region ◇ 전역 유닛 강화 정보 ◇
        public int UnitDamageLevel = 0;
        public int UnitAttackSpeedLevel = 0;
        public int UnitHpLevel = 0;
        public int UnitMoveSpeedLevel = 0;
        #endregion

        #region ◇ 개별 유닛 강화 정보 ◇
        /// <summary>
        /// 유닛 강화 레벨
        /// </summary>
        private Dictionary<string, int> unitUpgradeLevels = new(8);
        /// <summary>
        /// 유닛 생성 수 레벨
        /// </summary>
        private Dictionary<string, int> unitSizeLevels = new(8);
        #endregion

        #region ◇ 무기 강화 정보 ◇
        /// <summary>
        /// 무기 강화 레벨
        /// </summary>
        private Dictionary<string, int> weaponUpgradeLevels = new(10);
        #endregion

        #region ◇ 클리어 정보 ◇
        private HashSet<string> clearStageCodes = new();
        #endregion
        
        
        public void Init(RaceData data)
        {
            unitUpgradeLevels.Clear();
            unitSizeLevels.Clear();
            weaponUpgradeLevels.Clear();
            clearStageCodes.Clear();
            
            raceType = data.raceType;
            
            Money = data.initialData.money;
            Deck = data.initialData.firstDeck;
            foreach(UnitData unit in data.initialData.firstDeck)
            {
                unitUpgradeLevels.Add(unit.nameCode, 0);
            }
            Weapons = data.initialData.firstWeapons;
            foreach(WeaponData weapon in data.initialData.firstWeapons)
            {
                weaponUpgradeLevels.Add(weapon.nameCode, 0);
            }
            
            PlayerLevel = 0;
            
            PlayerHpLevel = 0;
            PlayerMoveSpeedLevel = 0;
            
            UnitDamageLevel = 0;
            UnitAttackSpeedLevel = 0;
            UnitHpLevel = 0;
            UnitMoveSpeedLevel = 0;

            CostRechargeLevel = 0;
            ManaRechargeLevel = 0;
            MaxCostLevel = 0;
            MaxManaLevel = 0;
            DeckLevel = 0;
        }


        public static string SaveGame()
        {
            string result = JsonConvert.SerializeObject(Instance);
            PlayerPrefs.SetString(SAVE_PREFS, result);
            
            return result;
        }

        public static bool LoadGame()
        {
            if (PlayerPrefs.HasKey(SAVE_PREFS))
            {
                LoadGame(PlayerPrefs.GetString(SAVE_PREFS));
                return true;
            }

            return false;
        }
        public static void LoadGame(string saveData) => Instance = JsonConvert.DeserializeObject<PlayerData>(saveData);
        

        public int GetUnitUpgradeLevel(string code) => unitUpgradeLevels.GetValueOrDefault(code, 0);
        public int GetUnitSizeLevel(string code) => unitSizeLevels.GetValueOrDefault(code, 0);
        public int GetWeaponUpgradeLevel(string code) => weaponUpgradeLevels.GetValueOrDefault(code, 0);
        
        public bool HasUnlockUnit(string code) => unitUpgradeLevels.ContainsKey(code);
        public void AddUnlockUnit(string code) => unitUpgradeLevels.Add(code, 0);
        
        public bool HasWeapon(string code) => weaponUpgradeLevels.ContainsKey(code);
        public void AddWeapon(string code) => weaponUpgradeLevels.Add(code, 0);
        
        public bool IsClearStage(string code) => clearStageCodes.Contains(code);
        public void AddClearStage(string code) => clearStageCodes.Add(code);
    }
}