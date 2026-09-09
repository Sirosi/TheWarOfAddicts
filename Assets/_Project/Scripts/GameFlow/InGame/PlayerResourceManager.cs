using System.Collections;
using DG.Tweening;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.GameFlow.InGame
{
    public class PlayerResourceManager: Singleton<PlayerResourceManager>, IBattleResultLockable
    {
        [SerializeField] private Slider costSlider;
        [SerializeField] private Slider manaSlider;

        
        public float CostRechargeDelay => PlayerData.Instance.CostRechargeDelay;
        public float ManaRechargeDelay => PlayerData.Instance.ManaRechargeDelay;
        
        public int MaxCost => PlayerData.Instance.MaxCost;
        public int MaxMana => PlayerData.Instance.MaxMana;

        public int Cost
        {
            get => cost;
            set
            {
                cost = value;
                costSlider.DOValue(cost, 0.5f);
            }
        }
        public int Mana
        {
            get => mana;
            set
            {
                mana = value;
                manaSlider.DOValue(mana, 0.5f);
            }
        }


        private int cost = 0;
        private int mana = 0;
        
        
        public void Init()
        {
            costSlider.maxValue = MaxCost;
            manaSlider.maxValue = MaxMana;
            Cost = PlayerData.Instance.StartCost;
            Mana = PlayerData.Instance.StartMana;
                
            StartCoroutine(RechargeCostCo());
            StartCoroutine(RechargeManaCo());
        }


        public void Lock()
        {
            StopAllCoroutines();
        }
        
        
        private IEnumerator RechargeCostCo()
        {
            while (true)
            {
                yield return new WaitForSeconds(CostRechargeDelay);

                Cost = Cost >= MaxCost ? MaxCost : Cost + 1;
            }
        }

        private IEnumerator RechargeManaCo()
        {
            while (true)
            {
                yield return new WaitForSeconds(ManaRechargeDelay);
                
                Mana = Mana >= MaxMana ? MaxMana : Mana + 1;
            }
        }
    }
}