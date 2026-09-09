using Lean.Pool;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.UI.InGame;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.UI.InGame
{
    public class HpBarManager: Singleton<HpBarManager>
    {
        [SerializeField] private HpBar hpBarPrefab;
        [SerializeField] private Transform hpBarGroup;


        // ReSharper disable Unity.PerformanceAnalysis
        public HpBar GetHpBar(Unit.Unit unit)
        {
            HpBar hpBar = LeanPool.Spawn(hpBarPrefab, hpBarGroup);
            hpBar.Target = unit;
            hpBar.Offset = unit.Data.hpBarOffset;
            if (unit.Owner == OwnerType.EnemyGroup)
            {
                hpBar.Offset += unit.Data.hpBarOffset.x * 2 * Vector2.left;
            }
            hpBar.Width = unit.Data.hpBarSize;

            return hpBar;
        }
    }
}