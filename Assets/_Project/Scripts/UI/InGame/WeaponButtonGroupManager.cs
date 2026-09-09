using System.Collections.Generic;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.Unit.Player.Weapon;
using UnityEngine;

namespace TheWarOfAddicts.UI.InGame
{
    public class WeaponButtonGroupManager : ButtonGroupManager<WeaponButton, WeaponData>
    {
        private const float DEFAULT_COOLDOWN = 0.5f;
        private static readonly KeyCode[] HoyKeys =
        {
            KeyCode.Z,
            KeyCode.X,
            KeyCode.C,
            KeyCode.V,
        };
        
        
        protected override void Awake()
        {
            base.Awake();
            CreateButtons(PlayerData.Instance?.Weapons);

            foreach (WeaponButton button in Buttons)
            {
                button.Manager = this;
            }
        }

        void Update()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Input.GetKeyDown(HoyKeys[i]))
                {
                    Buttons[i].ClickButton();
                }
            }
        }


        public void CoolingAllButtons(WeaponButton usingButton)
        {
            foreach (WeaponButton button in Buttons)
            {
                if (button.Interactable && button != usingButton)
                {
                    button.StartCooldown(DEFAULT_COOLDOWN);
                }
            }
        }
    }
}