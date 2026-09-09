using System;
using TheWarOfAddicts.Unit;
using UnityEngine;

namespace TheWarOfAddicts.GameFlow.StageData
{
    [Serializable]
    public class SpawnData
    {
        [Tooltip("생성할 유닛")] public UnitData data;
        [Tooltip("기준 유닛 생성 딜레이")] [Range(0.5f, 120f)] public float spawnInterval = 5;
        [Tooltip("기준 유닛 생성 딜레이")] [Range(0.5f, 120f)] public float startCooldown = 0;
    }
}