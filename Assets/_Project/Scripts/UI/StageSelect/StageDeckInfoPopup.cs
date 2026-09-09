using System.Collections.Generic;
using Lean.Pool;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.GameFlow.StageData;
using TheWarOfAddicts.Unit;
using UnityEngine;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class StageDeckInfoPopup: MonoBehaviour
    {
        [SerializeField] private StageInfoUnitIcon unitIconPrefab;
        [SerializeField] private RectTransform contentGroup;
        
        
        private Stack<StageInfoUnitIcon> unitIcons = new(8);
        
        
        public void Open()
        {
            gameObject.SetActive(true);

            StageData stageData = SessionData.SelectedStageData;
            
            while (unitIcons.Count > 0)
            {
                LeanPool.Despawn(unitIcons.Pop());
            }

            foreach (UnitData data in stageData.playerDeck)
            {
                StageInfoUnitIcon unitIcon = LeanPool.Spawn(unitIconPrefab, contentGroup);
                unitIcon.OpenInfo(data);
                
                unitIcons.Push(unitIcon);
            }
        }
    }
}