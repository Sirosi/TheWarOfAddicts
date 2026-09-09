using System.Collections;
using TheWarOfAddicts.GameFlow.InGame;
using TheWarOfAddicts.Unit;
using TheWarOfAddicts.Unit.UnitManage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.InGame
{
    public class SpawnButton: CooldownButton<UnitData>
    {
        protected override float Cooldown => Data?.cooldown ?? 1f;
        protected override bool IsReady => PlayerResourceManager.Instance.Cost >= Data.cost;
        
        
        public override void Init(UnitData data)
        {
            base.Init(data);
            
            icon.sprite = data.icon;
            costText.text = $"{data.cost}";
        }
        
        protected override void OnClick()
        {
            PlayerResourceManager.Instance.Cost -= Data.cost;
            UnitManager.Instance.SpawnAllyPawn(Data);
        }
    }
}