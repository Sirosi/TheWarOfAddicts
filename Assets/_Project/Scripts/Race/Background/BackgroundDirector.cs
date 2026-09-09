using System.Collections.Generic;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.GameFlow.InGame;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.GameFlow.StageData;
using UnityEngine;

namespace TheWarOfAddicts.Race.Background
{
    public class BackgroundDirector: Singleton<BackgroundDirector>, IBattleResultLockable
    {
        [SerializeField] private Transform backgroundGroup;


        private int leftIdx = 0;
        private int rightIdx = 0;
        
        private List<BackgroundProduct> allyBackgrounds = null;
        private List<BackgroundProduct> enemyBackgrounds = null;

        private StageData stageData = null;

        private bool isLocked = false;


        void LateUpdate()
        {
            // TODO: 추후에는 LateUpdate에서 매 프레임마다 동작시키는 게 아닌, Async or Coroutine 등으로 옮겨야 할듯 함.
            if (isLocked) return;
            
            float posX = stageData.mapWidth * InGameManager.Instance.CenterAmount;
            DeployAllyBackground(posX);
            DeployEnemyBackground(posX);
        }


        public void Init(PlayerData playerData)
        {
            stageData = SessionData.SelectedStageData;
            
            allyBackgrounds = new((int)(stageData.mapWidth / playerData.RaceData.backgroundData.intervalWidth));
            enemyBackgrounds = new((int)(stageData.mapWidth / stageData.enemyData.backgroundData.intervalWidth));
            
            // 아군 배경 배치
            float posX = stageData.mapWidth * InGameManager.MIN_AMOUNT + playerData.RaceData.backgroundData.intervalWidth * 0.5f;
            while (posX < stageData.mapWidth * InGameManager.MAX_AMOUNT)
            {
                BackgroundProduct prefab = playerData.RaceData.backgroundData.products[Random.Range(0, playerData.RaceData.backgroundData.products.Length)];
                BackgroundProduct product = Instantiate(prefab, backgroundGroup);

                product.gameObject.SetActive(false);
                product.transform.localPosition = Vector2.right * posX;
                allyBackgrounds.Add(product);

                posX += playerData.RaceData.backgroundData.intervalWidth;
            }

            // 적군 배경 배치
            posX = stageData.mapWidth * InGameManager.MIN_AMOUNT + stageData.enemyData.backgroundData.intervalWidth * 0.5f;
            while (posX < stageData.mapWidth * InGameManager.MAX_AMOUNT)
            {
                BackgroundProduct prefab = stageData.enemyData.backgroundData.products[Random.Range(0, stageData.enemyData.backgroundData.products.Length)];
                BackgroundProduct product = Instantiate(prefab, backgroundGroup);

                product.gameObject.SetActive(false);
                product.transform.localPosition = Vector2.right * posX;
                enemyBackgrounds.Add(product);
                
                posX += stageData.enemyData.backgroundData.intervalWidth;
            }
            enemyBackgrounds.Reverse();
        }


        public void Lock()
        {
            isLocked = true;
            
            StopAllCoroutines();
        }


        private void DeployAllyBackground(float posX)
        {
            while (leftIdx < allyBackgrounds.Count)
            {
                if (ProcessAllyBackground(allyBackgrounds[leftIdx], posX))
                {
                    leftIdx++;
                }
                else
                {
                    break;
                }
            }
            while (leftIdx > 0)
            {
                if (ProcessAllyBackground(allyBackgrounds[leftIdx - 1], posX))
                {
                    leftIdx--;
                }
                else
                {
                    break;
                }
            }
        }
        private void DeployEnemyBackground(float posX)
        {
            while (rightIdx < enemyBackgrounds.Count)
            {
                if (ProcessEnemyBackground(enemyBackgrounds[rightIdx], posX))
                {
                    rightIdx++;
                }
                else
                {
                    break;
                }
            }
            while (rightIdx > 0)
            {
                if (ProcessEnemyBackground(enemyBackgrounds[rightIdx - 1], posX))
                {
                    rightIdx--;
                }
                else
                {
                    break;
                }
            }
        }

        private bool ProcessAllyBackground(BackgroundProduct product, float posX)
        {
            // 표시 처리
            if (!product.gameObject.activeSelf && product.transform.position.x <= posX)
            {
                product.gameObject.SetActive(true);
                
                return true;
            }
            
            // 비표시 처리
            if (product.gameObject.activeSelf && product.transform.position.x > posX)
            {
                product.gameObject.SetActive(false);
                
                return true;
            }
                
            return false;
        }

        private bool ProcessEnemyBackground(BackgroundProduct product, float posX)
        {
            // 표시 처리
            if (!product.gameObject.activeSelf && product.transform.position.x >= posX)
            {
                product.gameObject.SetActive(true);
                
                return true;
            }
            
            // 비표시 처리
            if (product.gameObject.activeSelf && product.transform.position.x < posX)
            {
                product.gameObject.SetActive(false);
                
                return true;
            }
                
            return false;
        }
    }
}