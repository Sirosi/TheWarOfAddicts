using System;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Player.Weapon
{
    [Serializable]
    public class WeaponBaseData
    {
        [Header("기본 데이터")]
        [Tooltip("기본 성능")] public int influence = 10;
        [Tooltip("기본 쿨타임")] public float cooldown = 1f;
        [Tooltip("기본 비용")] public int cost = 10;
        
        [Header("투사체 정보")]
        [Tooltip("투사체 속도")] public float projectileSpeed = 2;
        [Tooltip("투사체 관통력, -1은 무한 관통")] public int impaleCount = 0;
    }
}