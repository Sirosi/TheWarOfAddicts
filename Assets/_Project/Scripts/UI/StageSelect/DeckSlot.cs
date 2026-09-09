using System;
using DG.Tweening;
using TheWarOfAddicts.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheWarOfAddicts.UI.StageSelect
{
    public class DeckSlot: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private const float ANIM_TIME = 0.1f;
        
        
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        
        
        public UnitData UnitData
        {
            get => unitData;
            set
            {
                this.unitData = value;

                if (unitData is null)
                {
                    iconImage.sprite = null;
                    iconImage.color = Color.clear;
                    nameText.text = string.Empty;
                }
                else
                {
                    iconImage.sprite = unitData.icon;
                    iconImage.color = Color.white;
                    nameText.text = unitData.nameCode;
                }
            }
        }
        
        
        public Action<DeckSlot, UnitData> OnClick = null;
        private UnitData unitData = null;
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this, unitData);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.1f, ANIM_TIME).SetEase(Ease.InOutCirc);
            AudioManager.Instance.PlayAudio(AudioType.MouseHover);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, ANIM_TIME).SetEase(Ease.InOutCirc);
            AudioManager.Instance.PlayAudio(AudioType.MouseExit);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, ANIM_TIME).SetEase(Ease.InOutCirc);
            AudioManager.Instance.PlayAudio(AudioType.MouseClick);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.1f, ANIM_TIME).SetEase(Ease.InOutCirc);
        }
    }
}