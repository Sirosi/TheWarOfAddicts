using System;
using System.Collections;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.InGame
{
    public class HpBar: MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private Image fillBar;
        
        
        public Unit.Unit Target { private get; set; }
        public Vector2 Offset { get; set; }
        public Color FillColor { set => fillBar.color = value; }
        public float Width { set => rect.sizeDelta = new Vector2(value, rect.sizeDelta.y); }
        
        
        private void LateUpdate()
        {
            transform.position = (Vector2)Target.transform.position + Offset;
        }

        public void Refresh(int current, int max)
        {
            float amount = current / (float)max;
            fillBar.fillAmount = amount;
        }
    }
}