using System.Collections.Generic;
using Lean.Pool;
using TheWarOfAddicts.GameFlow.StageData;
using TMPro;
using UnityEngine;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class StageInfoPopup: MonoBehaviour
    {
        [SerializeField] private StageInfoUnitIcon unitIconPrefab;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        [SerializeField] private RectTransform contentGroup;


        private Stack<StageInfoUnitIcon> unitIcons = new(15);


        public void OpenInfo(StageData stageData)
        {
            titleText.text = stageData.nameCode;
            descriptionText.text = $"전장너비: {stageData.mapWidth}";

            while (unitIcons.Count > 0)
            {
                LeanPool.Despawn(unitIcons.Pop());
            }

            foreach (SpawnData data in stageData.spawners)
            {
                StageInfoUnitIcon unitIcon = LeanPool.Spawn(unitIconPrefab, contentGroup);
                unitIcon.OpenInfo(data);
                
                unitIcons.Push(unitIcon);
            }

            // TODO: 스모키드는 Stage에 Deck정보가 있기 때문에 그것도 볼 수 있게 해줘야 함.
        }
    }
}