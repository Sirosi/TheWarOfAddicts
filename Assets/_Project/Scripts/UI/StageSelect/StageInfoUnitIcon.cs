using TheWarOfAddicts.GameFlow.StageData;
using TheWarOfAddicts.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class StageInfoUnitIcon: MonoBehaviour
    {
        // TODO: 클릭했을 때는 유닛 자세히 보기도 지원해야 함.
        
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI text;


        public void OpenInfo(SpawnData spawnData)
        {
            iconImage.sprite = spawnData.data.icon;
            text.text = $"{spawnData.spawnInterval:0}s({spawnData.startCooldown:0}s)";
        }

        public void OpenInfo(UnitData unitData)
        {
            iconImage.sprite = unitData.icon;
            text.text = unitData.nameCode;
        }
    }
}