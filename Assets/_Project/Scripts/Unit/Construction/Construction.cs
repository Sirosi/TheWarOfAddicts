using TheWarOfAddicts.UI.InGame;
using TheWarOfAddicts.UI.InGame.Map;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Construction
{
    public abstract class Construction: Unit
    {
        public new float MoveSpeed => 0f;


        void Start()
        {
            UnitManager.Instance.AddList(this);
        }


        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;
            
            base.Init(data, ownerType);
            
            HpBar = HpBarManager.Instance?.GetHpBar(this);
            HpBar?.Refresh(CurrentHp, MaxHp);
            //HpBar.FillColor = Color.blue;
                
            MapIcon = MapManager.Instance?.SetMapIcon(transform, Color.yellow, 2f);
        }
        
        protected override void OnDie()
        {
            // TODO: 파괴 이펙트 필요함.
        }
    }
}