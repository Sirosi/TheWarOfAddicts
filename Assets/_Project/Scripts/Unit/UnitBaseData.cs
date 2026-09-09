using System;
using UnityEngine;

namespace TheWarOfAddicts.Unit
{
    [Serializable]
    public class UnitBaseData
    {
        [Header("기본 데이터")]
        [Tooltip("최대 체력")] public int maxHp = 50;
        [Tooltip("기본 공격력")] public int damage = 3;
        [Tooltip("기본 방어력")] public int armor = 0;
        [Tooltip("기본 이동속도")] public float moveSpeed = 1;
        
        [Space(20f)]
        [Header("공격 데이터")]
        [Tooltip("기본 공격 최소 사거리")] public float attackMinRange = 0;
        [Tooltip("기본 공격 최대 사거리")] public float attackMaxRange = 1;
        [Tooltip("기본 공격속도")] public float attackSpeed = 1;
        [Tooltip("기본 스킬 확률")] public float skillRate = 0.1f;
        
        [Header("투사체 정보")]
        [Tooltip("투사체 속도")] public float projectileSpeed = 2;
        [Tooltip("투사체 관통력, -1은 무한 관통")] public int impaleCount = 0;
            
        [Header("특수 데이터")]
        [Tooltip("생성 가능 수")] public int size = 5;
    }
}