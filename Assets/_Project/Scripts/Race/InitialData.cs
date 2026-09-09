using System;
using TheWarOfAddicts.Unit;
using TheWarOfAddicts.Unit.Player.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheWarOfAddicts.Race
{
    [Serializable]
    public class InitialData
    {
        [Tooltip("초기 자금")] public int money = 0;
        [Tooltip("기본 덱 구성")] public UnitData[] firstDeck;
        
        [Tooltip("기본 무기 구성")] public WeaponData[] firstWeapons;
    }
}