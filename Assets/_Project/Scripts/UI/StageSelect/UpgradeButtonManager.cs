using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class UpgradeButtonManager: MonoBehaviour
    {
        [SerializeField] private UpgradeButton buttonPrefab;
        [SerializeField] private RectTransform buttonGroup;
        
        [SerializeField] private UpgradeData[] datas;


        private Stack<UpgradeButton> buttons = new(10);
        

        public void Open()
        {
            while(buttons.Count > 0)
            {
                LeanPool.Despawn(buttons.Pop());
            }
            
            foreach (UpgradeData data in datas)
            {
                UpgradeButton button = LeanPool.Spawn(buttonPrefab, buttonGroup);
                button.Init(data);
                
                buttons.Push(button);
            }
        }
    }
}