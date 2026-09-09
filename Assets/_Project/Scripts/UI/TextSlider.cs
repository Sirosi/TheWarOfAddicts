using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI
{
    public class TextSlider: Slider
    {
        [SerializeField] private TextMeshProUGUI text;


        protected override void Awake()
        {
            base.Awake();
            
            onValueChanged.AddListener(RefreshCostText);
        }


        private void RefreshCostText(float value)
        {
            text.text = $"{value:0} / {maxValue:0}";
        }
    }
}