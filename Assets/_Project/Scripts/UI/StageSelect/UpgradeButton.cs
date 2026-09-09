using System.Collections.Generic;
using System.Reflection;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.GameFlow.StageSelect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.StageSelect
{
    // Upgrade는 Reflection을 사용해서 PlayerData에 string기반으로 조회해서 진행함.
    public class UpgradeButton: MonoBehaviour
    {
        private const BindingFlags REFLECTION_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        
        
        private Button button = null;
        private int level = 0;
        private int price = 0;
        private UpgradeData upgradeData;


        void Awake()
        {
            button = GetComponent<Button>();
            
            button.onClick.AddListener(OnClick);
        }


        public void Init(UpgradeData data)
        {
            upgradeData = data;
            level = GetUpgradeLevel(upgradeData.upgradeCode, upgradeData.subCode);
            price = upgradeData.prices[0] >> 1;

            button.interactable = true;
            
            iconImage.sprite = upgradeData.icon;

            if (level < 0) // 비보유 상태
            {
                nameText.text = $"{upgradeData.nameCode}[비보유]";
                priceText.text = $"$ {price}";;
            }
            else if (level >= upgradeData.MaxLevel) // 레벨이 최대 상태
            {
                button.interactable = false;
                
                nameText.text = $"{upgradeData.nameCode}[MAX]";
                priceText.text = "최대 강화상태";
            }
            else
            {
                nameText.text = $"{upgradeData.nameCode}[{level + 1}]";
                priceText.text = $"$ {price = upgradeData.prices[level]}";;
            }
        }

        private void OnClick()
        {
            if (level >= upgradeData.MaxLevel || price > PlayerData.Instance.Money)
            {
                AudioManager.Instance.PlayAudio(AudioType.Deny);
                return;
            }
            
            string upgradeCode = upgradeData.upgradeCode;

            if (TryUpgrade(upgradeCode, upgradeData.subCode, level + 1))
            {
                AudioManager.Instance.PlayAudio(AudioType.Upgrade);
                print($"{upgradeCode}: 강화 성공");
                PlayerData.Instance.Money -= price;
                
                Init(upgradeData);
                StageSelectManager.Instance.RefreshMoney();
            }
            else
            {
                AudioManager.Instance.PlayAudio(AudioType.Deny);
                print($"{upgradeCode}: 강화 에러");
            }
        }


        private static bool TryUpgrade(string upgradeCode, string subCode, int newLevel)
        {
            FieldInfo fieldInfo = typeof(PlayerData).GetField(upgradeCode, REFLECTION_FLAGS);
            if (fieldInfo == null) return false; // 존재하지 않는 업그레이드 코드이므로 실패.

            if (fieldInfo.FieldType == typeof(int))
            {
                fieldInfo.SetValue(PlayerData.Instance, newLevel);
            }
            else if (fieldInfo.FieldType == typeof(Dictionary<string, int>))
            {
                Dictionary<string, int> dict = (Dictionary<string, int>)fieldInfo.GetValue(PlayerData.Instance);
                dict[subCode] = newLevel;
            }
            else
            {
                return false; // upgrade 타입이 아님.
            }

            return true;
        }

        private static int GetUpgradeLevel(string upgradeCode, string subCode)
        {
            FieldInfo fieldInfo = typeof(PlayerData).GetField(upgradeCode, REFLECTION_FLAGS);
            if (fieldInfo == null) return -1; // 존재하지 않는 업그레이드 코드이므로 실패.
            
            if (fieldInfo.FieldType == typeof(int))
            {
                return (int)fieldInfo.GetValue(PlayerData.Instance);
            }
            else if (fieldInfo.FieldType == typeof(Dictionary<string, int>))
            {
                Dictionary<string, int> dict = (Dictionary<string, int>)fieldInfo.GetValue(PlayerData.Instance);
                return dict.GetValueOrDefault(subCode, -1);
            }

            return -1;
        }
    }
}