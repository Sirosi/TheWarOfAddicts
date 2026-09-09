using System.Collections.Generic;
using Lean.Pool;
using TheWarOfAddicts.GameFlow;
using UnityEngine;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class WeaponUpgradeButtonManager: MonoBehaviour
    {
        [SerializeField] private UpgradeButton buttonPrefab;
        [SerializeField] private RectTransform buttonGroup;
        
        [SerializeField] private UpgradeData[] datas;


        private Stack<UpgradeButton> buttons = new(10);
        

        public void Open()
        {
            gameObject.SetActive(true);
            
            while(buttons.Count > 0)
            {
                LeanPool.Despawn(buttons.Pop());
            }
            
            foreach (UpgradeData data in datas)
            {
                if (PlayerData.Instance.RaceData.CanUseWeapon(data.subCode))
                {
                    UpgradeButton button = LeanPool.Spawn(buttonPrefab, buttonGroup);
                    button.Init(data);
                
                    buttons.Push(button);
                }
            }
        }
    }
}