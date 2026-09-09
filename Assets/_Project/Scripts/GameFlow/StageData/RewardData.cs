using System;
using UnityEngine;

namespace TheWarOfAddicts.GameFlow.StageData
{
    [Serializable]
    public class RewardData
    {
        [Tooltip("금전 보상")] public int money;
        
        
        public static RewardData operator +(RewardData a, RewardData b) => new RewardData()
        {
            money = a.money + b.money
        };
    }
}