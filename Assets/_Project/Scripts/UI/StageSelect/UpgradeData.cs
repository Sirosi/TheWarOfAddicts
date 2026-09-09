using UnityEngine;
using UnityEngine.UIElements;

namespace TheWarOfAddicts.UI.StageSelect
{
    [CreateAssetMenu(menuName = "Upgrade/New Upgrade Data", fileName = "New Upgrade Data")]
    public class UpgradeData: ScriptableObject
    {
        [Tooltip("아이콘")] public Sprite icon;
        
        [Tooltip("명칭")] public string nameCode;
        [Tooltip("업그레이드가 변경할 변수명")] public string upgradeCode;
        [Tooltip("무기명 등")] public string subCode;
        [Tooltip("비용")] public int[] prices;
        
        
        public int MaxLevel => prices.Length;
    }
}