using System.Collections;
using TheWarOfAddicts.GameFlow.InGame;
using TheWarOfAddicts.Unit.Player.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.InGame
{
    public class WeaponButton : CooldownButton<WeaponData>
    {
        public WeaponButtonGroupManager Manager { get; set; } = null;
        
        protected override float Cooldown => Data?.Cooldown ?? 1f;
        protected override bool IsReady => !InGameManager.Instance.Player.HasUsingWeapon && PlayerResourceManager.Instance.Mana >= Data.Cost;

        
        public override void Init(WeaponData data)
        {
            base.Init(data);
            
            icon.sprite = data.icon;
            costText.text = $"{data.Cost}";
        }
        
        protected override void OnClick()
        {
            PlayerResourceManager.Instance.Mana -= Data.Cost;
            InGameManager.Instance.Player.UseWeapon(Data);
            Manager.CoolingAllButtons(this);
        }
    }
}