using System;
using Lean.Pool;
using TheWarOfAddicts.GameFlow.StageData;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.InGame.Map
{
    public class MapUnitIcon: MonoBehaviour
    {
        private RectTransform Rect => image.rectTransform;
        
        
        private Image image;
        
        private RectTransform parentRect = null;
        private Transform target = null;
        private StageData stageData = null;


        void Awake()
        {
            image = GetComponent<Image>();
        }
        
        void LateUpdate()
        {
            if (target)
            {
                float amount = target.position.x / stageData.mapWidth;
                Rect.localPosition = parentRect.rect.width * amount * Vector3.right;
            }
        }


        public void Init(RectTransform parent, StageData stageData, Transform target, Color color, float scale)
        {
            parentRect = parent;
            this.stageData = stageData;
            this.target = target;
            image.color = color;
            Rect.localScale = scale * Vector3.one;
        }
    }
}