using TheWarOfAddicts.Core;
using TheWarOfAddicts.UI.StageSelect;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheWarOfAddicts.GameFlow.StageSelect
{
    public class StageSelectManager: Singleton<StageSelectManager>
    {
        [SerializeField] private GameObject[] stageMaps;
        
        [SerializeField] private StageInfoPopup stageInfoPopup;
        
        [SerializeField] private TextMeshProUGUI moneyText;
        
        [SerializeField] private Button localDeckButton;


        protected override void Awake()
        {
            base.Awake();
            
            localDeckButton.gameObject.SetActive(!PlayerData.Instance.RaceData.canUseDeck);

            for (int i = 0; i < stageMaps.Length; i++)
            {
                stageMaps[i].SetActive(i == (int)PlayerData.Instance.RaceData.raceType);
            }
        }

        void Start()
        {
            RefreshMoney();
        }

        public void OpenStageInfoPopup(StageData.StageData stageData)
        {
            SessionData.SelectedStageData = stageData;
            
            stageInfoPopup.gameObject.SetActive(true);
            stageInfoPopup.OpenInfo(stageData);
        }

        public void OpenUpgradeManager(UpgradeButtonManager upgradeButtonManager)
        {
            upgradeButtonManager.gameObject.SetActive(true);
            upgradeButtonManager.Open();
        }


        public void OnClickStartButton()
        {
            SceneManager.LoadScene("InGame");
        }

        public void RefreshMoney()
        {
            moneyText.text = $"{PlayerData.Instance.Money:N0}";
        }

        public void ReturnToLobby()
        {
            PlayerData.SaveGame();
            
            SceneManager.LoadScene("Lobby");
        }
    }
}