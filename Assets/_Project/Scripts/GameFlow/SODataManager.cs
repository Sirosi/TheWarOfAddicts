using System.Collections.Generic;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.Race;
using TheWarOfAddicts.Unit;
using TheWarOfAddicts.Unit.Player.Weapon;
using UnityEngine;

namespace TheWarOfAddicts.GameFlow
{
    public class SODataManager: Singleton<SODataManager>
    {
        [SerializeField] private RaceData[] raceData;
        
        [SerializeField] private UnitData[] unitData;
        [SerializeField] private WeaponData[] weaponData;


        private Dictionary<RaceType, RaceData> races = new();
        private Dictionary<string, UnitData> units = new();
        private Dictionary<string, WeaponData> weapons = new();
        

        protected override void Awake()
        {
            if(Instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            
            base.Awake();

            if (Instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }

            foreach (var r in raceData)
            {
                races.Add(r.raceType, r);
            }
            foreach (var unit in unitData)
            {
                units.Add(unit.nameCode, unit);
            }
            foreach (var weapon in weaponData)
            {
                weapons.Add(weapon.nameCode, weapon);
            }
        }
        
        
        public RaceData GetRaceData(RaceType raceType) => races.ContainsKey(raceType) ? races[raceType] : null;
        public UnitData GetUnitData(string code) => units.ContainsKey(code) ? units[code] : null;
        public WeaponData GetWeaponData(string code) => weapons.ContainsKey(code) ? weapons[code] : null;
    }
}