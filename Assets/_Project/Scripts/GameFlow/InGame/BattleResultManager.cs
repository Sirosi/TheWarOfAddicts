using System.Linq;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.GameFlow.StageData;
using TheWarOfAddicts.GameFlow.StageSelect;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheWarOfAddicts.GameFlow.InGame
{
    public class BattleResultManager: Singleton<BattleResultManager>
    {
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject defeatPanel;
        
        [SerializeField] private TextMeshProUGUI rewardText;
        
        
        private IBattleResultLockable[] lockables = null;
        private bool isOver = false;
        
        
        void Start()
        {
            lockables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IBattleResultLockable>().ToArray();
        }
        
        
        public void ProcessVictory()
        {
            if(!TryLockAll()) return;
            
            victoryPanel.SetActive(true);
            
            // 보상 합치기
            StageData.StageData currentStage = SessionData.SelectedStageData;
            RewardData reward = currentStage.defaultReward;
            if (!PlayerData.Instance.IsClearStage(currentStage.nameCode))
            {
                PlayerData.Instance.AddClearStage(currentStage.nameCode);
                reward += currentStage.onceReward;
            }

            // 보상 수령
            rewardText.text = $"{reward.money:N0}";
            PlayerData.Instance.Money += reward.money;

            PlayerData.SaveGame();
        }
        
        public void ProcessDefeat()
        {
            if(!TryLockAll()) return;
            
            defeatPanel.SetActive(true);
        }


        public void ClickConfirmButton()
        {
            StageData.StageData currentStage = SessionData.SelectedStageData;
            
            SceneManager.LoadScene(currentStage.battleType == BattleType.DefaultBattle ? "StageSelect" : "Epilogue");
        }

        public void BackToStageSelect()
        {
            SceneManager.LoadScene("StageSelect");
        }


        private bool TryLockAll()
        {
            if (isOver) return false;

            isOver = true;
            foreach (var lockable in lockables)
            {
                lockable.Lock();
            }

            return true;
        }
    }
}