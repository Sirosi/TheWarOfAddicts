using System.Collections;
using DG.Tweening;
using Lean.Pool;
using TheWarOfAddicts.Core;
using TheWarOfAddicts.GameFlow.InGame;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.InGame.Map
{
    public class MapManager: Singleton<MapManager>, IBattleResultLockable
    {
        [SerializeField] private Slider mapSlider;
        
        [SerializeField] private MapUnitIcon iconPrefab;
        [SerializeField] private RectTransform iconGroup;


        protected override void Awake()
        {
            base.Awake();
            
            mapSlider.maxValue = InGameManager.MAX_AMOUNT;
            mapSlider.minValue = InGameManager.MIN_AMOUNT;
        }

        void Start()
        {
            StartCoroutine(SlideMapSlider());
        }
        
        public MapUnitIcon SetMapIcon(Transform target, Color color, float scale = 1f)
        {
            
            MapUnitIcon icon = LeanPool.Spawn(iconPrefab, iconGroup);
            icon.Init(iconGroup, InGameManager.Instance.StageData, target, color, scale);

            return icon;
        }

        public void Lock()
        {
            StopAllCoroutines();
        }


        private IEnumerator SlideMapSlider()
        {
            WaitForSeconds wait = new WaitForSeconds(0.25f);
            while (true)
            {
                if (!Mathf.Approximately(mapSlider.value, InGameManager.Instance.CenterAmount))
                {
                    mapSlider.DOValue(InGameManager.Instance.CenterAmount, 0.5f);
                }
                
                yield return wait;
            }
        }
    }
}